using InvoiceProcessingPipeline.Application.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentMetadata : BoundaryContract
{
    public required string DocumentId { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required long Size { get; init; }
}