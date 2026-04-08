using Azure.Messaging;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Mapster;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations;

public sealed class CloudEventToDocumentEventMetadata : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CloudEvent, DocumentEventMetadata>()
            .MapWith(src => Create(src));
    }

    private static DocumentEventMetadata Create(CloudEvent src)
    {
        ArgumentNullException.ThrowIfNull(src);

        var source = src.Source?.ToString();

        return new DocumentEventMetadata
        {
            EventId = MappingGuard.Required(src.Id, "CloudEvent.Id"),
            EventType = MappingGuard.Required(src.Type, "CloudEvent.Type"),
            Source = MappingGuard.Required(source, "CloudEvent.Source"),
            EventTime = src.Time
        };
    }
}