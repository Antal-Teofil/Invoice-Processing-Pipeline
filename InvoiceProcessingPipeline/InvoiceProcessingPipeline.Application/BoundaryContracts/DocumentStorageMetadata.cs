using System.Diagnostics;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed record DocumentStorageMetadata
{
    public required string DocumentUrl { get; init; }
    public string? ContentType { get; init; }
    public string? BlobType { get; init; }
    public string? ETag { get; init; }
    public long? ContentLength { get; init; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay =>
        $"DocumentUrl={DocumentUrl ?? "<null>"}, " +
        $"ContentType={ContentType ?? "<null>"}, " +
        $"BlobType={BlobType ?? "<null>"}, " +
        $"ETag={ETag ?? "<null>"}, " +
        $"ContentLength={(ContentLength.HasValue ? ContentLength.Value.ToString() : "<null>")}";
}