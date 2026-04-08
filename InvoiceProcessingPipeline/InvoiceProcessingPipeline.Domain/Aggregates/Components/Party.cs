using InvoiceProcessingPipeline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.Aggregates.Components
{
    public sealed record Party : DocumentField
    {
        // seller's electronic address identifier
        // 1..1
        public required EndpointId EndpointId { get; set; }

        // 0..n
        public ICollection<PartyIdentification>? PartyIdentifications { get; set; }

        // 0..1
        public PartyName? Name { get; set; }

        // 1..1
        public required PostalAddress Address { get; set; }

        // 0..2
        public ICollection<PartyTaxScheme>? PartyTaxSchemes { get; set; }

        // 1..1
        public required PartyLegalEntity PartyLegalEntity { get; set; }

        // 0..1
        public Contact? ContactInfo { get; set; }
    }
}
