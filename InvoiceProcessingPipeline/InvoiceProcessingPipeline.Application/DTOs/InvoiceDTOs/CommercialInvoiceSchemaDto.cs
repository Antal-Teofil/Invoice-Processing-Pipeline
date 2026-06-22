using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record CommercialInvoiceSchemaDTO
    {
        [JsonPropertyName("header")]
        public DocumentMetadataDto? Metadata { get; set; }

        [JsonPropertyName("data")]
        public DocumentContentDto? Content { get; set; }
    }
}
