using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Mapster;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations;

public sealed class DocumentIngestionEventToDocumentEventRecord : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DocumentIngestionEvent, DocumentEventRecord>()
            .MapWith(src => Create(src));
    }

    private static DocumentEventRecord Create(DocumentIngestionEvent src)
    {
        ArgumentNullException.ThrowIfNull(src);
        ArgumentNullException.ThrowIfNull(src.EventMetadata);
        ArgumentNullException.ThrowIfNull(src.StorageMetadata);

        return new DocumentEventRecord
        {
            EventId = MappingGuard.Required(src.EventMetadata.EventId, "DocumentEventMetadata.EventId"),
            EventType = MappingGuard.Required(src.EventMetadata.EventType, "DocumentEventMetadata.EventType"),
            Source = MappingGuard.Required(src.EventMetadata.Source, "DocumentEventMetadata.Source"),
            EventTime = src.EventMetadata.EventTime,
            DocumentURL = MappingGuard.Required(src.StorageMetadata.DocumentUrl, "DocumentStorageMetadata.DocumentUrl"),
            ContentType = src.StorageMetadata.ContentType,
            BlobType = src.StorageMetadata.BlobType,
            ContentLength = src.StorageMetadata.ContentLength,
            ETag = src.StorageMetadata.ETag
        };
    }
}