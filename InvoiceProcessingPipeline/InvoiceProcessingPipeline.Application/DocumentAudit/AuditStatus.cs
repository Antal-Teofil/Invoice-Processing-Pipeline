namespace InvoiceProcessingPipeline.Application.DocumentAudit
{
    public enum AuditStatus : byte
    {
        EXTRACTED,
        FAILED,
        CONSTRAINT_VIOLATION,
        UNDER_REVIEW,
        REJECTED,
        APPROVED,
        BOOKED
    }
}
