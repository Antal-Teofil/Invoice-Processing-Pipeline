using System.Xml.Serialization;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record InvoiceNumber(string Value) : DocumentField;
}