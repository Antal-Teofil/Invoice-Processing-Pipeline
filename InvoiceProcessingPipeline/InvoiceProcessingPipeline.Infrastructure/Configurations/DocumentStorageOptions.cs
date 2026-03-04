using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations
{
    public sealed record DocumentStorageOptions
    {
        [ConfigurationKeyName("INVOICE_INBOUND_CONTAINER_NAME")]
        public required string DocumentInboundContainer { get; init; }

        [ConfigurationKeyName("SAS_TTL_IN_MINUTES")]
        public required int SasTtlMinutes { get; init; }
    }
}
