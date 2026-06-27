using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record CommercialInvoiceSchemaDto
    {
        [JsonPropertyName("header")]
        public required DocumentMetadataDto Metadata { get; set; }

        [JsonPropertyName("data")]
        public DocumentContentDto? Content { get; set; }
    }
}
