using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record QuantityUnitDto
    {
        [JsonPropertyName("amount")]
        public decimal? Value { get; set; }

        [JsonPropertyName("unitCode")]
        public string? Code { get; set; }
    }
}
