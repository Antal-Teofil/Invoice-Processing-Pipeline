namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record Price(PriceAmount PAmount, BaseQuantity? BaseQuantity, AllowanceCharge? AllowanceCharge) : DocumentField;
}
