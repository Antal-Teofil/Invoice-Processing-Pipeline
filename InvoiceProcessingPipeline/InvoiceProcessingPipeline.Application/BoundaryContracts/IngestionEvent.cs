using InvoiceProcessingPipeline.Application.Shared;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record IngestionEvent : BoundaryContract
{
    public required string EventId { get; init; }
    public required string EventType { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public required DateTime EventTime { get; init; }
    public required string DataVersion { get; init; }
    public required string Topic { get; init; }
    public required string TopicId { get; init; }
}