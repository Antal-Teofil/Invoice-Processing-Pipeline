namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentIngestionEvent
{
    public required string Id { get; init; }
    public required string CorrelationId { get; init; }
    public required DocumentStorageMetadata StorageMetadata { get; init; }
    public required DocumentEventMetadata EventMetadata { get; init; }
}