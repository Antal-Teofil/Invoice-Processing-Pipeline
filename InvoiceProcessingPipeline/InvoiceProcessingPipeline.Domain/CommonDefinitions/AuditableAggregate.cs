namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class AuditableAggregate
    {
        public required string WorkflowId { get; set; }
        public required string AuditStatus { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
    }
}
