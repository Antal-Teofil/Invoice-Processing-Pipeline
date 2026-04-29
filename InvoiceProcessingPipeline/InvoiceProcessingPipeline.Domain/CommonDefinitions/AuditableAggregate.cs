namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class AuditableAggregate
    {
        public string AuditStatus { get; set; } = default!;
    }
}
