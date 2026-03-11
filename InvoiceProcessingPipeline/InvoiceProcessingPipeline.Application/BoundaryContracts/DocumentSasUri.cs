namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record DocumentSasUri 
    { 
        public required Uri SasUri { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
