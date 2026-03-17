using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;


namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    // itt majd teszek egy `IOption<DocumentSchemaStoreOptions>`-t, hogy jol lehessen konfiguralni mit hova mentunk ha netan valami valtozik architektura szintjen, s szebb is
    public sealed class CosmosDocumentSchemaStore(ILogger<CosmosDocumentSchemaStore> logger, [FromKeyedServices("invoice-data")] Container storage) : IDocumentDataStore
    {
        public Task<DocumentDataSchema?> RetrieveCanonizedDocumentSchema(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ExtractedDocumentDataSchema?> RetrieveExtractedDocumentSchema(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpStatusCode> StoreCanonizedDocumentSchema(DocumentDataSchema schema)
        {
            var options = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            // itt is majd szepitunk es kitalalunk valami normalis partition key-t
            var response = await storage.CreateItemAsync(schema, new PartitionKey(schema.Id.ToString()), options);
            var status = response.StatusCode;
            return status;
        }

        public async Task<HttpStatusCode> StoreExtractedDocumentSchema(ExtractedDocumentDataSchema data)
        {
            // egyelore nem kell semmi plusz info a mentett objektumrol, ezert nem kerem vissza az objektumot a memoriaba
            // majd meglatjuk mit kezdunk az ETag-el.
            var options = new ItemRequestOptions // ezt majd beallitom globalisan is
            {
                EnableContentResponseOnWrite = false
            };
            // itt majd kicserelem a data.Id.Id-t valami egyebre, mert igy nem szep.
            var response = await storage.CreateItemAsync(data, new PartitionKey(data.Id.ToString()), options);
            var status = response.StatusCode;
            return status;
        }
    }
}
