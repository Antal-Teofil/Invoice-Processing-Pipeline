using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs
{
    public sealed record PartyTaxSchemeDto
    {
        [JsonPropertyName("companyId")]
        public string? CompanyId { get; set; }

        [JsonPropertyName("taxSchemeId")]
        public string? TaxSchemeId { get; set; }
    }
}
