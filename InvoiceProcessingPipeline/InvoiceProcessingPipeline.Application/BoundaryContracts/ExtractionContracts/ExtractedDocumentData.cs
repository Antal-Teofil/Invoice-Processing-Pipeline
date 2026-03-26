using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed class ExtractedDocumentData
    {
        public AnalyzerInformation? AnalyzerInformation { get; init; }

        public ExtractedDocumentFieldDictionary FieldDictionary { get; init; } = new();
    }
}
