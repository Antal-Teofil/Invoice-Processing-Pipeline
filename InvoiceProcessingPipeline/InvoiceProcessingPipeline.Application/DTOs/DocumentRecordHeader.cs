using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs
{
    public sealed record DocumentRecordHeader
    {
        [JsonPropertyName("documentAuditId")]
        public required string DocumentAuditId { get; init; }

        [JsonPropertyName("workflowId")]
        public required string WorkflowId { get; init; }

        [JsonPropertyName("auditStatus")]
        public required string AuditStatus { get; init; }

        [JsonPropertyName("updatedAt")]
        public required DateTimeOffset UpdatedAt { get; init; }
    }

}
