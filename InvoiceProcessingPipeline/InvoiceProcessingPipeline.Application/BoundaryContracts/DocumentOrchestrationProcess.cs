using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record DocumentOrchestrationProcess(DocumentOrchestrationProcessID Id, DocumentIngestionEvent docEvent) { }
}
