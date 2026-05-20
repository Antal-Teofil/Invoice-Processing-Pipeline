namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    // InvoiceId (AADI)
    public sealed record InvoiceNumber(string value) : DocumentField;
}
