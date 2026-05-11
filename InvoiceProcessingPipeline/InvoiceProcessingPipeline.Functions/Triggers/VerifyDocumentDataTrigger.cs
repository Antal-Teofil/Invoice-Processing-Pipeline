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
            var invoice = await store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(
                documentId);

            if (invoice is null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);

                await notFoundResponse.WriteAsJsonAsync(new
                {
                    message = $"Document with id {documentId} was not found."
                });

                return notFoundResponse;
            }

            var okResponse = req.CreateResponse(HttpStatusCode.OK);

            await okResponse.WriteAsJsonAsync(new
            {
                message = $"Document with id {documentId} was successfully retrieved.",
                data = invoice
            });

            return okResponse;
        }
    }
}