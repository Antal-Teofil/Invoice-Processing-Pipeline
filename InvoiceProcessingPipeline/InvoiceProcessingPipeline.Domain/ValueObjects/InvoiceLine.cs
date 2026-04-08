namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record InvoiceLine(string LineId, Note? LineNote, InvoicedQuantity Quantity, LineExtensionAmount LEAmount, Item Kind, Price ItemPrice);
}
