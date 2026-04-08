using System.Diagnostics;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed record DocumentIngestionEvent
{
    public required string Id { get; init; }
    public required string CorrelationId { get; init; }
    public required DocumentStorageMetadata StorageMetadata { get; init; }
    public required DocumentEventMetadata EventMetadata { get; init; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay =>
        $"Id={Id ?? "<null>"}, " +
        $"CorrelationId={CorrelationId ?? "<null>"}, " +
        $"EventId={EventMetadata?.EventId ?? "<null>"}, " +
        $"EventType={EventMetadata?.EventType ?? "<null>"}, " +
        $"DocumentUrl={StorageMetadata?.DocumentUrl ?? "<null>"}";
}