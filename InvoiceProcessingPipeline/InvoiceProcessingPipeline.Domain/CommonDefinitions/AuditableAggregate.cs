namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class AuditableAggregate
    {
        public required string AuditStatus { get; set; } = default!;
    }
}
