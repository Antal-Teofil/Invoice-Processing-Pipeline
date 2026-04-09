namespace InvoiceProcessingPipeline.Application.Validations
{
    public sealed record SchemaViolation
    {
        public string? ViolationMessage {  get; set; }
    }
}
