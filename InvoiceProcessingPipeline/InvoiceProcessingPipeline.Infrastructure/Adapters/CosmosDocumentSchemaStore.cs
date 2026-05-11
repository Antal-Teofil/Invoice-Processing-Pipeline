using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;


namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    // itt majd teszek egy `IOption<DocumentSchemaStoreOptions>`-t, hogy jol lehessen konfiguralni mit hova mentunk ha netan valami valtozik architektura szintjen, s szebb is

    public sealed class CosmosDocumentSchemaStore(ILogger<CosmosDocumentSchemaStore> logger, [FromKeyedServices("invoice-data")] Container storage) : IDocumentDataStore
    {
        public async Task<TDocumentType> RetrieveCanonicalizedDocumentSchemeAsync<TDocumentType>(string documentId) where TDocumentType : DocumentScheme
        {
            var response = await storage.ReadItemAsync<TDocumentType>(documentId, new PartitionKey(documentId));
            var resource = response.Resource;
            return resource;
        }

        public async Task<ExtractedDocumentData?> RetrieveExtractedDocumentSchemaAsync(string id)
        {
            ItemResponse<ExtractedDocumentData> response = await storage.ReadItemAsync<ExtractedDocumentData>(id, new PartitionKey(id));
            return response.Resource;
        }

        public async Task<PagedResult<ExtractedDocumentData>> RetrievePagedExtractedDocumentSchemaAsync(
            int pageSize,
            string? continuationToken,
            CancellationToken token = default)
        {
            QueryDefinition queryDefinition = new("SELECT * FROM c");

            QueryRequestOptions queryRequestOptions = new()
            {
                MaxItemCount = pageSize,
            };

            using FeedIterator<ExtractedDocumentData> iterator =
                storage.GetItemQueryIterator<ExtractedDocumentData>(
                    queryDefinition: queryDefinition,
                    continuationToken: continuationToken,
                    requestOptions: queryRequestOptions);

            if (!iterator.HasMoreResults)
            {
                return new PagedResult<ExtractedDocumentData>(
                    [],
                    null);
            }

            FeedResponse<ExtractedDocumentData> response =
                await iterator.ReadNextAsync(token);

            return new PagedResult<ExtractedDocumentData>(
                [.. response],
                response.ContinuationToken);
        }

        public async Task<HttpStatusCode> StoreCanonicalizedDocumentSchemeAsync<TDocumentType>(TDocumentType documentScheme) where TDocumentType : DocumentScheme
        {
            var response = await storage.CreateItemAsync(documentScheme, new PartitionKey(documentScheme.DocumentId.ToString()));
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> StoreExtractedDocumentSchemaAsync(ExtractedDocumentData data)
        {
            // egyelore nem kell semmi plusz info a mentett objektumrol, ezert nem kerem vissza az objektumot a memoriaba
            // majd meglatjuk mit kezdunk az ETag-el.
            var options = new ItemRequestOptions // ezt majd beallitom globalisan is
            {
                EnableContentResponseOnWrite = false
            };
            // itt majd kicserelem a data.Id.Id-t valami egyebre, mert igy nem szep.
            var response = await storage.CreateItemAsync(data, new PartitionKey(data.Id), options);
            var status = response.StatusCode;
            return status;
        }
    }
}
