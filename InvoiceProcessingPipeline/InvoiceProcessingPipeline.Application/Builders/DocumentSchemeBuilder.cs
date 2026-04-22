using InvoiceProcessingPipeline.Domain.CommonDefinitions;

namespace InvoiceProcessingPipeline.Application.Builders
{
    public class DocumentSchemeBuilder<TSource> where TSource: DocumentDataScheme, new()
    {
        private readonly TSource documentScheme = new();
        public TSource BuildScheme()
        {
            return documentScheme;
        }
    }
}
