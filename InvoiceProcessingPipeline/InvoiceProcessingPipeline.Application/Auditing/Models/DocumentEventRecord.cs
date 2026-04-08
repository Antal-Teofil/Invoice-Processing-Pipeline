namespace InvoiceProcessingPipeline.Application.Auditing.Models;

public sealed record DocumentEventRecord
{
    public required string EventId { get; init; }
    public required string EventType { get; init; }
    public required string Source { get; init; }
    public DateTimeOffset? EventTime { get; init; }
    public required string DocumentURL { get; init; }
    public string? ContentType { get; init; }
    public string? BlobType { get; init; }
    public long? ContentLength { get; init; }
    public string? ETag { get; init; }
}