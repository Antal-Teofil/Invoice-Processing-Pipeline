using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentDataScheme : AuditableEntity
    {
        // rendszer beli dokumentum id
        public required Guid DocumentId { get; set; } = Guid.NewGuid();

        // 1..1
        public required string CustomizationId { get; set; } = default!;

        // 1..1
        public required string ProfileId { get; set; } = default!;
    }
}
