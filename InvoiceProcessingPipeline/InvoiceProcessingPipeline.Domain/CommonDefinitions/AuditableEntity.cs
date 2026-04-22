namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class AuditableEntity
    {
        public required string AuditStatus { get; set; } = default!;
    }
}
