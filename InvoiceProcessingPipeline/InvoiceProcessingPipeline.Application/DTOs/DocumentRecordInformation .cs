using InvoiceProcessingPipeline.Application.DocumentAudit;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs
{
    /// <summary>
    /// Represents the most crucial information about an invoice record. This record is used to display the invoice records in the UI and to provide a summary of the invoice records for auditing purposes.
    /// </summary>
    public sealed record DocumentRecordInformation
    {
        [JsonPropertyName("header")]
        public required DocumentRecordHeader Header { get; init; }

        [JsonPropertyName("data")]
        public required DocumentRecordData Data { get; init; }
    }
}
