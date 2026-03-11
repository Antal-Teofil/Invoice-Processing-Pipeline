namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentStorageMetadata
{
    public required string DocumentUrl { get; init; }
    public string? ContentType { get; init; }
    public string? BlobType { get; init; }
    public string? ETag { get; init; }
    public long? ContentLength { get; init; }
}