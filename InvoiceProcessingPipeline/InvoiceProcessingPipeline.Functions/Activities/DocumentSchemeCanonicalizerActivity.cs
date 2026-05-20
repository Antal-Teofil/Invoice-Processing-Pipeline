using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace InvoiceProcessingPipeline.Functions.Activities
{
    public class DocumentSchemeCanonicalizerActivity(ILogger<DocumentSchemeCanonicalizerActivity> logger, IDocumentDataStore storage)
    {
        [Function(nameof(DocumentSchemeCanonicalizerActivity))]
        public async Task<ActivityResult<string>> RunAsync([ActivityTrigger] string input)
        {
            var extraction = await storage.RetrieveExtractedDocumentSchemaAsync(input);

            if(extraction is null)
            {
                return ActivityResult<string>.Success($"Extraction failed with id: {input}");
            }

            logger.LogInformation($"Document to be canonicalized with id: {extraction.DocumentId}");

            var invoice = DocumentScheme.From(extraction).AsCommercialInvoice()
                .AddInvoiceNumber()
                .AddDocumentCurrencyCode()
                .AddAccountingCustomerParty()
                .AddAccountingSupplierParty()
                .AddIssueDate()
                .AddDueDate()
                .AddInvoiceLines()
                .AddLegalMonetaryTotal()
                .Build();

            if(invoice is null)
            {
                return ActivityResult<string>.Success($"Canonicalization failed with id: {input}");
            }

            await storage.StoreCanonicalizedDocumentSchemeAsync(invoice);

            logger.LogInformation($"Canonicalized document is stored with id: {invoice.DocumentId}");

            return ActivityResult<string>.Success(input);
        }
    }
}
