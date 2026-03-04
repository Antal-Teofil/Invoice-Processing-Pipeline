using InvoiceProcessingPipeline.Application.Auditing;
using InvoiceProcessingPipeline.Application.Auditing.Models;
using InvoiceProcessingPipeline.Application.Auditing.Ports;
using InvoiceProcessingPipeline.Infrastructure.Configurations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class CosmosDocumentAuditStorage(CosmosClient client, IOptions<CosmosAuditOptions> option) : IDocumentAuditStore
    {
        public async Task EnsureExistance()
        {
            var options = option.Value;

            Database database = await client.CreateDatabaseIfNotExistsAsync(options.InvoiceAuditDatabase);
            
            await database.CreateContainerIfNotExistsAsync(options.InvoiceAuditContainer, options.PartitionKey);
            await database.CreateContainerIfNotExistsAsync(options.InvoiceEventContianer, options.PartitionKey);
            await database.CreateContainerIfNotExistsAsync(options.InvoiceSchemaContainer, options.PartitionKey);
        }

        public async Task<RecordStatus> RecordEvent(DocumentEventRecord record)
        {
            await EnsureExistance();

            var itemResposne = await client.GetContainer(option.Value.InvoiceEventContianer, option.Value.PartitionKey).CreateItemAsync(record, new PartitionKey(record.EventId));
            
            bool exists = itemResposne.StatusCode == HttpStatusCode.OK;

            return new RecordStatus(exists);
        }
    }
}
