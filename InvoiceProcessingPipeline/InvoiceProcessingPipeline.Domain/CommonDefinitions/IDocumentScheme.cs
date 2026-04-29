using InvoiceProcessingPipeline.Domain.ExtractionContracts;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public interface IDocumentScheme<TDocument> where TDocument : DocumentScheme
    {
        static abstract DocumentSchemeBuilder<TDocument> Create(ExtractedDocumentData extraction);
    }
}
