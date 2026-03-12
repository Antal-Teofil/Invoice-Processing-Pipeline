using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record DueDate()
    {
        public required DateOnly Date { get; init; }
    }
}
