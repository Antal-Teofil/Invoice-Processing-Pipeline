using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record AmountUnitDto
    {
        [JsonPropertyName("amount")]
        public decimal? Value { get; set; }

        [JsonPropertyName("currencyCode")]
        public string? Code { get; set; }
    }
}
