namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record InvoiceLine(string LineId, Note? LineNote, InvoicedQuantity InvoicedQuantity, LineExtensionAmount LineExtensionAmount, InvoicePeriod? InvoicePeriod, Item Kind, Price ItemPrice, ICollection<AllowanceCharge>? AllowanceCharges) : DocumentField;
}
