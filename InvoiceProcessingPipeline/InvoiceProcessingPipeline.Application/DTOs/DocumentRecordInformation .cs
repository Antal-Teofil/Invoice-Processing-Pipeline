using InvoiceProcessingPipeline.Application.DocumentAudit;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs
{
    /// <summary>
    /// Represents the most crucial information about an invoice record. This record is used to display the invoice records in the UI and to provide a summary of the invoice records for auditing purposes.
    /// </summary>
    public sealed record DocumentRecordInformation
    {
        /// <summary>
        /// The status of the document in the invoice processing workflow
        /// </summary>
        [JsonPropertyName("auditStatus")]
        [JsonConverter(typeof(JsonStringEnumConverter<AuditStatus>))]
        public required AuditStatus AuditStatus { get; set; }

        /// <summary>
        /// Identifies the workflow process id which is basically the id of the durable proccess
        /// </summary>
        [JsonPropertyName("workflowId")]
        public required string ProcessId { get; set; }

        /// <summary>
        /// Unique identifier of the document in the process
        /// </summary>
        [JsonPropertyName("documentAuditId")]
        public required string DocumentId { get; set; }

        /// <summary>
        /// Unique identifier of the invoice in contability context
        /// </summary>
        [JsonPropertyName("invoiceNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InvoiceId { get; set; }

        /// <summary>
        /// The name of the supplier party of the invoice
        /// </summary>
        [JsonPropertyName("accountingSupplierParty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? VendorName { get; set; }

        /// <summary>
        /// The phone number of the supplier party
        /// </summary>
        [JsonPropertyName("supplierPhoneNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The e-mail address of the supplier party
        /// </summary>
        [JsonPropertyName("supplierEmailAddress")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? VendorEmailAddress { get; set; }

        /// <summary>
        /// The total amount of the invoice lines with tax
        /// </summary>
        [JsonPropertyName("totalAmount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// Document Currency Code
        /// </summary>
        [JsonPropertyName("currencyCode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CurrencyCode { get; set; }

        [JsonPropertyName("updatedAt")]
        public required DateTimeOffset UpdatedAt { get; set; }

        [JsonPropertyName("auditor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ReviewedBy { get; set; }
    }
}
