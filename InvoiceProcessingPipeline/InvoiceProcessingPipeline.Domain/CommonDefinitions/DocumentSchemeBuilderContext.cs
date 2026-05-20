using InvoiceProcessingPipeline.Domain.ExtractionContracts;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public sealed class DocumentSchemeBuilderContext
    {
        public ExtractedDocumentData Extraction { get; }

        internal DocumentSchemeBuilderContext(ExtractedDocumentData extraction)
        {
            ArgumentNullException.ThrowIfNull(extraction);
            Extraction = extraction;
        }

        public CommercialInvoiceDocumentBuilder AsCommercialInvoice()
        {
            return new CommercialInvoiceDocumentBuilder(Extraction);
        }
    }
}