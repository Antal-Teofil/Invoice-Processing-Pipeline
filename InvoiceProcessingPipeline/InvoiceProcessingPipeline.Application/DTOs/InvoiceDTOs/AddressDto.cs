using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed class AddressDto
    {
        [JsonPropertyName("streetName")]
        public string? StreetName { get; set; }

        [JsonPropertyName("additionalStreetName")]
        public string? AdditionalStreetName { get; set; }

        [JsonPropertyName("cityName")]
        public string? City { get; set; }

        [JsonPropertyName("postalZone")]
        public string? PostalZone { get; set; }

        [JsonPropertyName("countrySubentity")]
        public string? CountrySubentity { get; set; }

        [JsonPropertyName("addressLine")]
        public string? AddressLine { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }
    }
}
