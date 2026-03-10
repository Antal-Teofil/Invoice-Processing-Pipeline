using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class DocumentEventOrchestratorService(ILogger<DocumentEventOrchestratorService> logger, [FromKeyedServices("invoice-event")] Container eventContainer) : IDocumentEventOrchestrator
    {
        public Task RecordDocumentOrchestrationEvent(DocumentOrchestrationTask docProcess)
        {
            throw new NotImplementedException();
        }

        // records an incoming document event
        // to be refactored
        public async Task RecordEvent(DocumentIngestionEvent eventRecord)
        {
            ArgumentNullException.ThrowIfNull(eventRecord);

            try
            {
                await eventContainer.CreateItemAsync(item: eventRecord, partitionKey: new PartitionKey(eventRecord.Id));

                logger.LogInformation("Document event recorded.\nEventID: {EventId}\nDocumentURL: {DocumentURL}", eventRecord.EventMetadata.EventId, eventRecord.StorageMetadata.DocumentURL);
            }
            catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogWarning("Document event was already emitted.\n EventID: {EventId}\n", eventRecord.EventMetadata.EventId);
            }
            catch (CosmosException exeption) {
                throw;
            }
        }

        public Task RecordEventAsync(DocumentIngestionEvent eventRecord)
        {
            throw new NotImplementedException();
        }

        public ValueTask<DocumentIngestionEvent?> RetrieveEventRecordAsync(EventID Id)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<DocumentOrchestrationTaskID> StartDocumentOrchestrationAsync(string TaskName)
        {
            DocumentOrchestrationTaskID id = 
        }

        public ValueTask<bool> VerifyEventRecordExistanceAsync(EventID Id)
        {
            throw new NotImplementedException();
        }
    }
}
