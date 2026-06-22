using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record LineItemDto
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("classifiedTaxCategory")]
        public TaxCategoryDto? Category { get; set; }
    }
}
