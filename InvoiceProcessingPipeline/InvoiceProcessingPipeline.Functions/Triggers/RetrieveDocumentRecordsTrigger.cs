using InvoiceProcessingPipeline.Application.DocumentAudit;
using InvoiceProcessingPipeline.Application.Ports;
using MapsterMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    // retrieves invoices under processing
    public sealed class RetrieveDocumentRecordsTrigger(ILogger<RetrieveDocumentRecordsTrigger> logger, IDocumentDataStore docStore, IMapper mapper)
    {
        [Function(nameof(RetrieveDocumentRecordsTrigger))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "audit/records")] HttpRequestData request, [DurableClient] DurableTaskClient client)
        {

            var query = new OrchestrationQuery
            {
                Statuses = [OrchestrationRuntimeStatus.Pending, OrchestrationRuntimeStatus.Running],
                FetchInputsAndOutputs = true,
                PageSize = 100
            };

            var matchesCustomStatus = new List<DocumentAuditSnapshot>();
            await foreach (var instance in client.GetAllInstancesAsync(query))
            {
                try
                {
                    var customDeserializedStatus = instance.ReadCustomStatusAs<DocumentAuditSnapshot>();

                    if(customDeserializedStatus is null)
                    {
                        logger.LogInformation("Deserialization returned with null value!");
                        continue;
                    }
                    var status = customDeserializedStatus.AuditStatus;

                    if (status is AuditStatus.CONSTRAINT_VIOLATION or AuditStatus.EXTRACTED or AuditStatus.UNDER_REVIEW)
                    {
                        matchesCustomStatus.Add(customDeserializedStatus);
                    }
                }
                catch(JsonSerializationException exception)
                {
                    logger.LogInformation(exception, "Custom audit snapshot deserialization failed");
                    
                    continue;
                }
                
            }



            logger.LogInformation("Information retrieved");
            var response = request.CreateResponse(System.Net.HttpStatusCode.OK);
            return response;
        }
    }
}
