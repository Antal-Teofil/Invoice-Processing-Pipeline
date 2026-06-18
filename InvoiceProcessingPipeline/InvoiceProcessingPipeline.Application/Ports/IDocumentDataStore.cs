using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using System.Net;

namespace InvoiceProcessingPipeline.Application.Ports
{
    // majd bevezetem a Result<> patternt ide, mert kelleni fog.
    public interface IDocumentDataStore
    {
        // itt is Result<> lesz nem csunya HttpStatusCode
        public Task<HttpStatusCode> StoreExtractedDocumentSchemaAsync(ExtractedDocumentData data);
        public Task<ExtractedDocumentData?> RetrieveExtractedDocumentSchemaAsync(string id);
        public Task<HttpStatusCode> StoreCanonicalizedDocumentSchemeAsync<TDocumentType>(TDocumentType documentScheme) where TDocumentType: DocumentScheme;
        public Task<TDocumentType> RetrieveCanonicalizedDocumentSchemeAsync<TDocumentType>(string documentId) where TDocumentType : DocumentScheme;
        public Task ReplaceCanonicalizedDocumentSchemeAsync<TDocumentType>(TDocumentType correctedDocument) where TDocumentType : DocumentScheme;
        public Task<PagedResult<TDocument>> RetrievePagedDocumentCollectionAsync<TDocument>(int pageSize, string? continuationToken, CancellationToken cancellationToken) where TDocument : DocumentScheme;
    }
}
