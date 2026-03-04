using Azure.Messaging.EventGrid.SystemEvents;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations
{
    public sealed class StorageBlobCreatedEventDataToDocumentStorageMetadata : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<StorageBlobCreatedEventData, DocumentStorageMetadata>()
                .MapWith(s => new DocumentStorageMetadata
                {
                    DocumentURL = s.Url,
                    ContentType = s.ContentType,
                    BlobType = s.BlobType,
                    ETag = s.ETag,
                    ContentLength = s.ContentLength
                });
        }
    }
}
