using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record Address()
    {
        public required string StreetName { get; init; }
        public required string City { get; init; }
        public required string PostalCode { get; init; }
        public required string CountryCode { get; init; }
    }
}
