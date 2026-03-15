using InvoiceProcessingPipeline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed record ExtractedDocumentDataSchema()
    {
        public ExtractedField<AnalyzerInformation>? AnalyzerInformation { get; set; }
    }
}
