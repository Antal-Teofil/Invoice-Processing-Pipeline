using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using Microsoft.Azure.Functions.Worker.Core.Invocation;
using System.Net;

namespace InvoiceProcessingPipeline.Application.Ports
{
    // majd bevezetem a Result<> patternt ide, mert kelleni fog.
    public interface IDocumentDataStore
    {
        // itt is Result<> lesz nem csunya HttpStatusCode
        public Task<HttpStatusCode> StoreExtractedDocumentSchemaAsync(ExtractedDocumentData data);
        public Task<HttpStatusCode> StoreCanonizedDocumentSchemaAsync(DocumentDataSchema schema);
        public Task<DocumentDataSchema?> RetrieveCanonizedDocumentSchemaAsync(string id);
        public Task<ExtractedDocumentData?> RetrieveExtractedDocumentSchemaAsync(string id);
        public Task<PagedResult<ExtractedDocumentData>> RetrievePagedExtractedDocumentSchema(int pageSize, string? ContinuationToken, CancellationToken token = default);
    }
}
