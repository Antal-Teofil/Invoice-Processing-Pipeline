using InvoiceProcessingPipeline.Application.Auditing.Models;
using InvoiceProcessingPipeline.Application.Auditing.Ports;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Auditing
{
    public sealed class DocumentAuditOrchestrator(IDocumentAuditStore storage)
    {
        public async Task<AuditResult<RecordStatus>> RecordDocumentIngestionEventAsync(DocumentIngestionEvent documentEvent, CancellationToken token)
        {
            var eventRecord = documentEvent.Adapt<DocumentEventRecord>();
            var status = await storage.RecordEvent(eventRecord);
            return new AuditResult<RecordStatus> { Value = status };
        }
    }
}
