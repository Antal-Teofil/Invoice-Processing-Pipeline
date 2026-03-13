using InvoiceProcessingPipeline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.Aggregates.Components
{
    public sealed record Party()
    {
        public required Address PartyAddress { get; init; }
    }
}
