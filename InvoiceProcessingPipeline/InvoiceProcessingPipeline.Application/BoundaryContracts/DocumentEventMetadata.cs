using System.Diagnostics;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed record DocumentEventMetadata
{
    public required string EventId { get; init; }
    public required string EventType { get; init; }
    public required string Source { get; init; }
    public DateTimeOffset? EventTime { get; init; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay =>
        $"EventId={EventId ?? "<null>"}, " +
        $"EventType={EventType ?? "<null>"}, " +
        $"Source={Source ?? "<null>"}, " +
        $"EventTime={(EventTime.HasValue ? EventTime.Value.ToString("O") : "<null>")}";
}