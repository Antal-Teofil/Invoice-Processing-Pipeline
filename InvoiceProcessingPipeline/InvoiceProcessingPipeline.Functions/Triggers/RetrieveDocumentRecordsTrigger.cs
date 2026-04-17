using InvoiceProcessingPipeline.Application.DocumentAudit;
using InvoiceProcessingPipeline.Application.DTOs;
using InvoiceProcessingPipeline.Application.Ports;
using MapsterMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    // retrieves invoices under processing
    public sealed class RetrieveDocumentRecordsTrigger(
        ILogger<RetrieveDocumentRecordsTrigger> logger,
        IDocumentDataStore storage,
        IMapper mapper)
    {
        [Function(nameof(RetrieveDocumentRecordsTrigger))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "audit/records")] HttpRequestData request,
            [DurableClient] DurableTaskClient client)
        {
            var query = new OrchestrationQuery
            {
                Statuses =
                [
                    OrchestrationRuntimeStatus.Pending,
                    OrchestrationRuntimeStatus.Running
                ],
                FetchInputsAndOutputs = true,
                PageSize = 100
            };

            // processId -> audit status
            var activeProcesses = new Dictionary<string, AuditStatus>(StringComparer.Ordinal);

            await foreach (var instance in client.GetAllInstancesAsync(query))
            {
                try
                {
                    var customStatus = instance.ReadCustomStatusAs<DocumentAuditSnapshot>();

                    if (customStatus is null)
                    {
                        logger.LogInformation("Custom status deserialization returned null for orchestration {InstanceId}.", instance.InstanceId);
                        continue;
                    }

                    var status = customStatus.AuditStatus;

                    if (status is AuditStatus.CONSTRAINT_VIOLATION
                        or AuditStatus.EXTRACTED
                        or AuditStatus.UNDER_REVIEW)
                    {
                        activeProcesses[instance.InstanceId] = status;
                    }
                }
                catch (JsonSerializationException exception)
                {
                    logger.LogInformation(exception, "Custom audit snapshot deserialization failed for orchestration {InstanceId}.", instance.InstanceId);
                }
            }

            var results = new List<DocumentRecordInformation>();

            string? continuationToken = null;

            do
            {
                var page = await storage.RetrievePagedExtractedDocumentSchema(
                    pageSize: 100,
                    continuationToken: continuationToken);

                foreach (var document in page.Items)
                {
                    if (string.IsNullOrWhiteSpace(document.ProcessId))
                    {
                        continue;
                    }

                    if (!activeProcesses.TryGetValue(document.ProcessId, out var auditStatus))
                    {
                        continue;
                    }

                    var dto = mapper.Map<DocumentRecordInformation>((Document: document, AuditStatus: auditStatus));

                    results.Add(dto);
                }

                continuationToken = page.ContinuationToken;
            }
            while (!string.IsNullOrEmpty(continuationToken));

            logger.LogInformation("Retrieved {Count} document records.", results.Count);

            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(results);
            return response;
        }
    }
}