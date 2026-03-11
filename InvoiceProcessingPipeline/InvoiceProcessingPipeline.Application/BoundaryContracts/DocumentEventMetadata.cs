namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentEventMetadata
{
    public required string EventId { get; init; }
    public required string EventType { get; init; }
    public required string Source { get; init; }
    public DateTimeOffset? EventTime { get; init; }
}