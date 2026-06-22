using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record InvoiceLineDto
    {
        [JsonPropertyName("lineId")]
        public string? Id { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("invoicedQuantity")]
        public QuantityUnitDto? Quantity { get; set; }

        [JsonPropertyName("lineExtensionAmount")]
        public AmountUnitDto? ExtensionAmount { get; set; }

        [JsonPropertyName("invoicePeriod")]
        public InvoicePeriodDto? Period { get; set; }

        [JsonPropertyName("item")]
        public LineItemDto? LineItem { get; set; }

        [JsonPropertyName("price")]
        public PriceUnitDto? PriceUnit { get; set; }
    }
}
