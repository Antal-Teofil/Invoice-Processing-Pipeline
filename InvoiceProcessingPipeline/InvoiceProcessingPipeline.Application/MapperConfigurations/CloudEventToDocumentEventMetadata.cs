using Azure.Messaging;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations
{
    public sealed class CloudEventToDocumentEventMetadata : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CloudEvent, DocumentEventMetadata>()
                .MapWith(src => new DocumentEventMetadata
                {
                    EventId = src.Id,
                    EventType = src.Type,
                    Source = src.Source,
                    EventTime = src.Time
                });
        }
    }
}
