
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed class AccountingPartyDto
    {
        [JsonPropertyName("partyName")]
        public string? Name { get; set; }

        [JsonPropertyName("postalAddress")]
        public AddressDto? Address { get; set; }

        [JsonPropertyName("partyTaxScheme")]
        public ICollection<PartyTaxSchemeDto>? TaxScheme { get; set; }

        [JsonPropertyName("partyLegalEntity")]
        public PartyLegalEntityDto? LegalEntity { get; set; }

        [JsonPropertyName("contact")]
        public PartyContactDto? Contact { get; set; }
    }
}
