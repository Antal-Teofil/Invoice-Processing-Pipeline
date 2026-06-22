using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record TaxSubtotalDto
    {
        [JsonPropertyName("taxableAmount")]
        public AmountUnitDto? TaxableAmount { get; set; }

        [JsonPropertyName("taxAmount")]
        public AmountUnitDto? TaxAmount { get; set; }

        [JsonPropertyName("taxCategory")]
        public TaxCategoryDto? TaxCategory { get; set; }
    }
}
