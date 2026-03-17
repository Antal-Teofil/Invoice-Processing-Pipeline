using InvoiceProcessingPipeline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed record ExtractedDocumentDataSchema()
    {
        public required Guid Id {  get; set; }
        public ExtractedField<AnalyzerInformation>? AnalyzerInformation { get; set; }
    }
}
