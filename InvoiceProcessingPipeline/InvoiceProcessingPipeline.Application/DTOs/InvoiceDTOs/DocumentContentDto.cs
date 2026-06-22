using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record DocumentContentDto
    {
        [JsonPropertyName("invoiceNumber")]
        public string? InvoiceNumber { get; set; }

        [JsonPropertyName("issueDate")]
        public DateOnly? IssueDate { get; set; }

        [JsonPropertyName("dueDate")]
        public DateOnly? DueDate { get; set; }

        [JsonPropertyName("typeCode")]
        public string? InvoiceTypeCode { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("taxPointDate")]
        public DateOnly? TaxPointDate { get; set; }

        [JsonPropertyName("documentCurrencyCode")]
        public string? DocumentCurrencyCode { get; set; }

        [JsonPropertyName("taxCurrencyCode")]
        public string? TaxCurrencyCode { get; set; }

        [JsonPropertyName("invoicePeriod")]
        public InvoicePeriodDto? InvoicePeriod { get; set; }

        [JsonPropertyName("accountingPartyScheme")]
        public AccountingPartyDto? Vendor { get; set; }

        [JsonPropertyName("accountingSupplierParty")]
        public AccountingPartyDto? Customer { get; set; }

        [JsonPropertyName("taxTotal")]
        public ICollection<TaxTotalDto>? Total { get; set; }

        [JsonPropertyName("legalMonetaryTotal")]
        public MonetaryTotalDto? MonetaryTotal { get; set; }

        [JsonPropertyName("invoiceLine")]
        public ICollection<InvoiceLineDto>? InvoiceLines { get; set; }
    }
}
