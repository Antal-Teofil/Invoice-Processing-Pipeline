using InvoiceProcessingPipeline.Application.Ports;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    public sealed class VerifyDocumentDataTrigger(ILogger<VerifyDocumentDataTrigger> logger, IDocumentDataStore store)
    {
        [Function(nameof(VerifyDocumentDataTrigger))]
        public async Task RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "verify/{invoiceId}")] HttpRequestData req, string invoiceId)
        {

            Application.BoundaryContracts.ExtractionContracts.ExtractedDocumentData? data = await store.RetrieveExtractedDocumentSchemaAsync(invoiceId);

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(json);
        }
    }
}
