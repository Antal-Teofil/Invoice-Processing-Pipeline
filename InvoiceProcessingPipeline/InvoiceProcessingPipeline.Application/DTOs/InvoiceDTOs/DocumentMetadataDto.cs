using InvoiceProcessingPipeline.Application.DocumentAudit;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record DocumentMetadataDto
    {
        [JsonPropertyName("documentAuditId")]
        public required Guid DocumentId { get; init; }

        [JsonPropertyName("workflowId")]
        public required Guid WorkflowId { get; init; }

        [JsonPropertyName("auditStatus")]
        public required AuditStatus AuditStatus { get; init; }

        [JsonPropertyName("updatedAt")]
        public required DateTimeOffset UpdatedAt { get; init; }
    }
}

