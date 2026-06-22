using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record PartyLegalEntityDto
    {
        [JsonPropertyName("registrationName")]
        public string? Name { get; set; }

        [JsonPropertyName("companyId")]
        public string? CompanyId { get; set; }

        [JsonPropertyName("companySchemeId")]
        public string? SchemeId { get; set; }

        [JsonPropertyName("comapnyLegalForm")]
        public string? LegalForm { get; set; }
    }
}
