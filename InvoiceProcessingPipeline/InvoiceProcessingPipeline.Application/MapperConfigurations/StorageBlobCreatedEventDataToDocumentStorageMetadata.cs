using Azure.Messaging.EventGrid.SystemEvents;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Mapster;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations;

public sealed class StorageBlobCreatedEventDataToDocumentStorageMetadata : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StorageBlobCreatedEventData, DocumentStorageMetadata>()
            .MapWith(src => Create(src));
    }

    private static DocumentStorageMetadata Create(StorageBlobCreatedEventData src)
    {
        ArgumentNullException.ThrowIfNull(src);

        return new DocumentStorageMetadata
        {
            DocumentUrl = MappingGuard.Required(src.Url, "StorageBlobCreatedEventData.Url"),
            ContentType = src.ContentType,
            BlobType = src.BlobType,
            ETag = src.ETag,
            ContentLength = src.ContentLength
        };
    }
}