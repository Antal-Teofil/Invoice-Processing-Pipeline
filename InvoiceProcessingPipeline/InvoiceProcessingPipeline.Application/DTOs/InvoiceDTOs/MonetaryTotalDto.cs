using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record MonetaryTotalDto
    {
        [JsonPropertyName("lineExtensionAmount")]
        public AmountUnitDto? LineExtension { get; set; }

        [JsonPropertyName("taxExclusiveAmount")]
        public AmountUnitDto? TaxExclusive { get; set; }

        [JsonPropertyName("allowanceTotalAmount")]
        public AmountUnitDto? AllowanceTotal { get; set; }

        [JsonPropertyName("chargeTotalAmount")]
        public AmountUnitDto? ChargeTotal { get; set; }

        [JsonPropertyName("prepaidAmount")]
        public AmountUnitDto? Prepaid { get; set; }

        [JsonPropertyName("payableRoundingAmount")]
        public AmountUnitDto? PayableRounding { get; set; }

        [JsonPropertyName("payableAmount")]
        public AmountUnitDto? PayableAmount { get; set; }
    }
}
