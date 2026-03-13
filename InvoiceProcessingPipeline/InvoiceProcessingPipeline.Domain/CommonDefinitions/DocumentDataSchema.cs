using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentDataSchema : AuditableEntity
    {
        public required DocumentIdentifier DocumentIdentifier { get; init; }
    }
}
