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
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "verify/{invoiceId}")] HttpRequestData req, string invoiceId)
        {
            logger.LogInformation(
                "Verifying extracted document data for invoice {InvoiceId}.",
                invoiceId);

            ExtractedDocumentData? data =
                await store.RetrieveExtractedDocumentSchemaAsync(invoiceId);

            if (data is null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);

                await notFoundResponse.WriteStringAsync(
                    $"No extracted document data found for invoice '{invoiceId}'.");

                return notFoundResponse;
            }

            var document = DocumentScheme.From(data).As<CommercialInvoice>()
                .Build();

            string json = JsonConvert.SerializeObject(
                document,
                Formatting.Indented);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            await response.WriteStringAsync(json);

            return response;
        }
    }
}