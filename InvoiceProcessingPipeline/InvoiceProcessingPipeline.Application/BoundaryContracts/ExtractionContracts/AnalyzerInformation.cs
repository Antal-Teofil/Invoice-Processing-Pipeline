using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed class AnalyzerInformation
    {
        public required string ApiInfo { get; set; }
    }
}
