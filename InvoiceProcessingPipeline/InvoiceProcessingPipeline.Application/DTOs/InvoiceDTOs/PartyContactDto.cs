using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record PartyContactDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("telephone")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("electronicMail")]
        public string? EmailAddress { get; set; }
    }
}
