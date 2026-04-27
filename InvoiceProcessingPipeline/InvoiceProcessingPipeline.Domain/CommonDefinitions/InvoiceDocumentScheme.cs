
using InvoiceProcessingPipeline.Domain.EnumTypes;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class InvoiceDocumentScheme : DocumentScheme
    {   
        public abstract InvoiceTypeCode TypeCode { get; protected set; }
    }
}
