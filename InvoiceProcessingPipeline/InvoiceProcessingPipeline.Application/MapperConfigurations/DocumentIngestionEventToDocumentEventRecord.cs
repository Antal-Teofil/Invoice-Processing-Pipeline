using InvoiceProcessingPipeline.Application.Auditing.Models;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations
{
    public sealed class DocumentIngestionEventToDocumentEventRecord : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<DocumentIngestionEvent, DocumentEventRecord>()
                .MapWith(src => new DocumentEventRecord
                {
                    EventId = src.EventMetadata.EventId,
                    EventType = src.EventMetadata.EventType,
                    Source = src.EventMetadata.Source,
                    EventTime = src.EventMetadata.EventTime,
                    DocumentURL = src.StorageMetadata.DocumentURL,
                    ContentType = src.StorageMetadata.ContentType,
                    BlobType = src.StorageMetadata.BlobType,
                    ContentLength = src.StorageMetadata.ContentLength,
                    ETag = src.StorageMetadata.ETag
                });
        }
    }
}
