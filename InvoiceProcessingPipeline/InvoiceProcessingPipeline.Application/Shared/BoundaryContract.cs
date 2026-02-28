using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Shared;

public abstract record BoundaryContract
{
    public required string CorrelationId { get; init; }
}