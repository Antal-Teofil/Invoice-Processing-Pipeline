using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations
{
    public sealed record CosmosAuditOptions
    {
        [ConfigurationKeyName("PARTITION_KEY")]
        public required string PartitionKey { get; set; }

        [ConfigurationKeyName("AUDIT_DATABASE")]
        public required string InvoiceAuditDatabase { get; init; }

        [ConfigurationKeyName("AUDIT_CONTAINER")]
        public required string InvoiceAuditContainer { get; init; }

        [ConfigurationKeyName("EVENT_CONTAINER")]
        public required string InvoiceEventContianer { get; init; }

        [ConfigurationKeyName("SCHEMA_CONTAINER")]
        public required string InvoiceSchemaContainer { get; init; }
    }
}
