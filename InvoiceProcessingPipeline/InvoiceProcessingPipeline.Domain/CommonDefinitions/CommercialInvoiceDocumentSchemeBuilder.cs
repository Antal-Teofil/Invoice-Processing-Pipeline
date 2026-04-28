
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public sealed class CommercialInvoiceDocumentSchemeBuilder : InvoiceDocumentSchemeBuilder
    {
        public CommercialInvoiceDocumentScheme DocumentScheme { get; private set; }
        public override InvoiceDocumentScheme Build()
        {
            return DocumentScheme;
        }
    }
}
