using Castle.Core.Logging;
using InvoiceProcessingPipeline.Application.Ports;
using MapsterMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Web;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    // retrieves invoices under processing
    public sealed class RetrieveDocumentRecordsTrigger(ILogger<RetrieveDocumentRecordsTrigger> logger, IDocumentDataStore docStore, IMapper mapper)
    {
        [Function(nameof(RetrieveDocumentRecordsTrigger))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "invoices")] HttpRequestData request)
        {
            
            logger.LogInformation("Information retrieved");
            var response = request.CreateResponse(System.Net.HttpStatusCode.OK);



            return response;
        }
    }
}
