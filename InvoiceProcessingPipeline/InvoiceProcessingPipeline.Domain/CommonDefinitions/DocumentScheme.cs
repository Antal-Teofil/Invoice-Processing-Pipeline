using InvoiceProcessingPipeline.Domain.ExtractionContracts;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentScheme : AuditableAggregate
    {
        public required Guid DocumentId { get; init; } = Guid.NewGuid();

        public static DocumentSchemeBuilderContext From(ExtractedDocumentData extraction)
        {
            return new DocumentSchemeBuilderContext(extraction);
        }
    }
}
