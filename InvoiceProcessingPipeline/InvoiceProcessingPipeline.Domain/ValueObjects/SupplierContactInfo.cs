using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record SupplierContactInfo()
    {
        public required string EmailAddress { get; init; }
        public required string PhoneNumber { get; init; }
        public required string ContactName { get; init; }
    }
}
