
using Castle.Core.Logging;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public sealed class ConvertToXmlDocumentActivity(ILogger<ConvertToXmlDocumentActivity> logger, IDocumentDataStore store, IDocumentSchemeExporter exporter)
    {
        [Function(nameof(ConvertToXmlDocumentActivity))]
        public async Task RunAsync([ActivityTrigger] string documentId)
        {

            var document = await store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(documentId);

            if(document is null)
            {
                logger.LogInformation("Document with id {DocumentId} not found in the data store.", documentId);
                return;
            }

            await exporter.Export(document);
        }
    }
}
