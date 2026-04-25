using InvoiceProcessingPipeline.Domain.CommonDefinitions;

namespace InvoiceProcessingPipeline.Application.Builders
{
    public sealed record DocumentSchemeBuilder<TSource> where TSource: DocumentScheme, new()
    {
    }
}
