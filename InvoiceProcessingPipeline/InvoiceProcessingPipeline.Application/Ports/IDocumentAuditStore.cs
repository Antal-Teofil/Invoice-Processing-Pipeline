using InvoiceProcessingPipeline.Application.DTOs;
using InvoiceProcessingPipeline.Application.Validations;
using System.Net;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentAuditStore
    {
        public Task<string> StoreIssueRecord(Accumulator issues, string documentId);
        public Task<IssueRecord> RetrieveIssueRecord(string issueRecordId);
    }
}
