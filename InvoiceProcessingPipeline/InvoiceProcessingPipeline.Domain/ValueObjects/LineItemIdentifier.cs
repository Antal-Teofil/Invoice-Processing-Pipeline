using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record LineItemIdentifier()
    {
        public required string Id { get; init; }
    }
}
