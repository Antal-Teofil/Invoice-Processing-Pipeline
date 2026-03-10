using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record DocumentOrchestrationTaskID(string Id) 
    {
        public static implicit operator DocumentOrchestrationTaskID(string id) => new(id);
    }
}
