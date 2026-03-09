using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations
{
    public sealed record DocumentStorageOptions
    {
        public required int SasTtlMinutes { get; init; }
    }
}
