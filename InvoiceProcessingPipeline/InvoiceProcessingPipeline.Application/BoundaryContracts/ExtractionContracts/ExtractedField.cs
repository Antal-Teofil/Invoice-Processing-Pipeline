using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed record ExtractedField<TSource> where TSource : class
    {
        public required string FieldName { get; set; }
        public required string FieldValue { get; set; }
        public required TSource Field { get; set; }
        public required float ConfidenceScore { get; set; }
    }
}
