namespace InvoiceProcessingPipeline.Domain.ExtractionContracts
{
    public sealed record AnalyzerWarning
    {
        public required string Code { get; init; }

        public string? Message { get; init; }

        public required string Target { get; init; }
    }
}
