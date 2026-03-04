using InvoiceProcessingPipeline.Application.Shared;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentIngestionEvent
{
    public required string CorrelationId { get; init; }
    public required DocumentStorageMetadata StorageMetadata { get; set; }
    public required DocumentEventMetadata EventMetadata { get; set; }
}