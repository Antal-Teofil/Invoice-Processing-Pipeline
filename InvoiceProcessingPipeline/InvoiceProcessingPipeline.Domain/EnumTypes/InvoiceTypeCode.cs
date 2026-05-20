using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.EnumTypes
{
    public sealed record InvoiceTypeCode(string Value) : DocumentField;
}
