using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    public sealed class VerifyDocumentDataTrigger(
        ILogger<VerifyDocumentDataTrigger> logger,
        IDocumentDataStore store)
    {
        [Function(nameof(VerifyDocumentDataTrigger))]
        public async Task<HttpResponseData> RunAsync(
    [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "verify/{documentId}")]
    HttpRequestData req,
    string documentId)
        {
            var invoice = await store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(documentId);

            
        }
    }
}