using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed class AnalyzerInformation
    {
        public required string ApiVersion { get; init; }

        public required string ModelId { get; init; }

        public IReadOnlyCollection<AnalyzerWarning>? Warnings { get; init; }
    }
}
