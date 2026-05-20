namespace InvoiceProcessingPipeline.Domain.ExtractionContracts
{
    public sealed class AnalyzerInformation
    {
        public required string ApiVersion { get; init; }

        public required string ModelId { get; init; }

        public IReadOnlyCollection<AnalyzerWarning>? Warnings { get; init; }
    }
}
