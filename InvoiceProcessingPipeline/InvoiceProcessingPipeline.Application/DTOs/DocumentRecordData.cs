using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs
{
    public sealed record DocumentRecordData
    {
        [JsonPropertyName("invoiceNumber")]
        public string? InvoiceNumber { get; init; }

        [JsonPropertyName("accountingSupplierParty")]
        public string? AccountingSupplierParty { get; init; }

        [JsonPropertyName("supplierPhoneNumber")]
        public string? SupplierPhoneNumber { get; init; }

        [JsonPropertyName("supplierEmailAddress")]
        public string? SupplierEmailAddress { get; init; }

        [JsonPropertyName("totalAmount")]
        public decimal? TotalAmount { get; init; }

        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; init; }

        [JsonPropertyName("auditor")]
        public string? Auditor { get; init; }
    }
}
