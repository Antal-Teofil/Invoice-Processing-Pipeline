using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using System.Net;

namespace InvoiceProcessingPipeline.Application.Ports
{
    // majd bevezetem a Result<> patternt ide, mert kelleni fog.
    public interface IDocumentDataStore
    {
        // itt is Result<> lesz nem csunya HttpStatusCode
        public Task<HttpStatusCode> StoreExtractedDocumentSchema(ExtractedDocumentData data);
        public Task<HttpStatusCode> StoreCanonizedDocumentSchema(DocumentDataSchema schema);
        public Task<DocumentDataSchema?> RetrieveCanonizedDocumentSchema(string id);
        public Task<ExtractedDocumentData?> RetrieveExtractedDocumentSchema(string id);
    }
}
