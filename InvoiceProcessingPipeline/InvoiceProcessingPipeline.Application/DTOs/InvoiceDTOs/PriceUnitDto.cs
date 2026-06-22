using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record PriceUnitDto
    {
        [JsonPropertyName("priceAmount")]
        public AmountUnitDto? Price { get; set; }

        [JsonPropertyName("baseQuantity")]
        public QuantityUnitDto? Quantity { get; set; }
    }
}
