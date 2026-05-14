using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.DTOs;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Validations;
using InvoiceProcessingPipeline.Infrastructure.Configurations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class CosmosDocumentAuditStore([FromKeyedServices("invoice-audit")] Container store, IOptions<CosmosAuditOptions> option) : IDocumentAuditStore
    {

        public async Task<IssueRecord> RetrieveIssueRecord(string issueRecordId, string documentId)
        {
            var response = await store.ReadItemAsync<IssueRecord>(
                issueRecordId,
                new PartitionKey(documentId)
            );

            return response.Resource;
        }

        public async Task<string> StoreIssueRecord(Accumulator issues, string documentId)
        {
            IssueRecord issueRecord = new()
            {
                Id = Guid.NewGuid().ToString(),
                Issues = issues,
                RecordDate = DateTime.UtcNow,
                DocumentId = documentId
            };

            var response = await store.CreateItemAsync(
                issueRecord,
                new PartitionKey(issueRecord.Id)
            );

            return issueRecord.Id;
        }
    }
}
