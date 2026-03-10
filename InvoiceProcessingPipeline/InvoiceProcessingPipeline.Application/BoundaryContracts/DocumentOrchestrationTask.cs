using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record DocumentOrchestrationTask(DocumentOrchestrationTaskID Id, DocumentIngestionEvent docEvent) { }
}
