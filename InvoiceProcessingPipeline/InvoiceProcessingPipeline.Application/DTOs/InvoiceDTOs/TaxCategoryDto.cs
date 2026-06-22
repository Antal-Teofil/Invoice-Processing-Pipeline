using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record TaxCategoryDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("percent")]
        public double? Percent { get; set; }

        [JsonPropertyName("taxScheme")]
        public string? TaxScheme { get; set; }
    }
}
