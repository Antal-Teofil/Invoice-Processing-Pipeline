using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record ExtractedDocumentResponse
    {
        public required string ExtractedDocumentId { get; set; }
    }
}
