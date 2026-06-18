using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DocumentAudit
{
    public sealed record DocumentAuditSnapshot
    {
        [JsonPropertyName("documentId")]
        public required string DocumentId { get; set; }

        [JsonPropertyName("workflowId")]
        public required string OrchestrationId { get; set; }

        [JsonPropertyName("auditStatus")]
        public required AuditStatus AuditStatus { get; set; }
    }
}
