using InvoiceProcessingPipeline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record ExtractedDocumentDataSchema()
    {
        public required DocumentIdentifier Id {  get; init; }
    }
}
