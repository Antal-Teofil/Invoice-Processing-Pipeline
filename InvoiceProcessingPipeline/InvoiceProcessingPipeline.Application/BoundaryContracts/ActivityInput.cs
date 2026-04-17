using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record ActivityInput
    {
        // javitas, itt majd nem lehet nullable
        public DocumentUserDelegationSasUri? SasUri { get; set; }
        public required string ProcessId { get; set; }
    }
}
