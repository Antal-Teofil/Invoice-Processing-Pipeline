using InvoiceProcessingPipeline.Application.Auditing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Auditing.Ports
{
    public interface IDocumentAuditStore
    {
        public Task<RecordStatus> RecordEvent(DocumentEventRecord record);
        public Task EnsureExistance();
    }
}
