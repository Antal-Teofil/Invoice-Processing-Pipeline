using InvoiceProcessingPipeline.Application.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentStorageMetadata
{
    public required string DocumentURL { get; init; }
    public required string ContentType { get; init; }
    public required string BlobType { get; init; }
    public required string ETag { get; init; }
    public long? ContentLength { get; init; }
}