using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.DocumentAudit
{
    public record DocumentAuditSnapshot()
    {
        public string? DocumentId { get; set; }
        public string? OrchestrationId { get; set; }
        public string? CorrelationId { get; set; }
        public required AuditStatus AuditStatus { get; set; }
    }
}
