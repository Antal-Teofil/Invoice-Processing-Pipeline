
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts
{
    public sealed record DocumentCorrectionSubmitted
    {
        public required CommercialInvoice correction { get; init; }

        public required string instanceId { get; init; }
    }
}
