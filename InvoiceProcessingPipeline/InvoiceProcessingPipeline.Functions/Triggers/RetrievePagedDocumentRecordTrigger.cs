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
    public sealed class RetrievePagedDocumentRecordTrigger(ILogger<RetrievePagedDocumentRecordTrigger> logger, IDocumentDataStore docStore, IMapper mapper)
    {
        [Function(nameof(RetrievePagedDocumentRecordTrigger))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "invoices")] HttpRequestData request)
        {
            var query = HttpUtility.ParseQueryString(request.Url.Query);

            logger.LogInformation("Query parameters are parsed");
             
            var pageSize = int.TryParse(query["pageSize"], out var parsedPageSize) ? parsedPageSize : 20;

            var continuationToken = query["continuationToken"];

            var result = await docStore.RetrievePagedExtractedDocumentSchema(pageSize, continuationToken);

            logger.LogInformation("Information retrieved");
            var response = request.CreateResponse(System.Net.HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
