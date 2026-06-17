using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public sealed class ExportXmlDocumentActivity(ILogger<ExportXmlDocumentActivity> logger, IDocumentDataStore store, IDocumentSchemeExporter exporter, IDocumentXmlStore xmlStore)
    {
        [Function(nameof(ExportXmlDocumentActivity))]
        public async Task RunAsync([ActivityTrigger] string documentId)
        {

            var document = await store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(documentId);

            if(document is null)
            {
                logger.LogInformation("Document with id {DocumentId} not found in the data store.", documentId);
                return;
            }

            var exportedXmlDocument = await exporter.ExportAsync(document);

            await xmlStore.StoreXmlDocumentSchemeAsync(exportedXmlDocument);
        }
    }
}
