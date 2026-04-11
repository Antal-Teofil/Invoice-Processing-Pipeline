using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.DocumentAudit
{
    public record DocumentAuditSnapshot
    {
        public required string DocumentId { get; set; }
        public required string OrchestrationId { get; set; }
        public string? CorrelationId { get; set; }
        public required AuditStatus AuditStatus { get; set; }
    }
}
