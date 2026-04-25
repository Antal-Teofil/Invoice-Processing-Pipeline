
using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.EnumTypes;
using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class InvoiceDocumentScheme : DocumentScheme
    {
        public abstract void ConvertToUblXml();
    }
}
