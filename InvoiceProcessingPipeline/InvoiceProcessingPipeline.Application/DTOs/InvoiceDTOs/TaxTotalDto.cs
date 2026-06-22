using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record TaxTotalDto
    {
        [JsonPropertyName("taxAmount")]
        public AmountUnitDto? Amount { get; set; }

        [JsonPropertyName("taxSubtotal")]
        public ICollection<TaxSubtotalDto>? Subtotal { get; set; }
    }
}
