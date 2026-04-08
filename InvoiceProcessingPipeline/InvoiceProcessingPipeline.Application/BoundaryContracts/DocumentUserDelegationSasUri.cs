namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record DocumentUserDelegationSasUri 
    { 
        public required Uri SasUri { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
