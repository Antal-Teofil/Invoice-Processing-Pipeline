using InvoiceProcessingPipeline.Application.Enums;

namespace InvoiceProcessingPipeline.Application.DTOs
{
    public sealed record DocumentRecordMetadataResponse
    {
        public required string DocumentId { get; init; }
        public required string ProcessId { get; init; }
        public required IngestionStatus IngestionStatus { get; init; }
        public string? VendorName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTimeOffset? IssueDate { get; set; }
    }
}