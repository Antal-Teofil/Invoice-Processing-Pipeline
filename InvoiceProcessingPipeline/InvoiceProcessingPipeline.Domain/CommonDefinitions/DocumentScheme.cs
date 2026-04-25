using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentScheme : AuditableAggregate
    {
        // rendszer beli dokumentum id
        public required Guid DocumentId { get; set; } = Guid.NewGuid();
    }
}
