using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record IssueDate()
    {
        public required string DateOnly { get; init; }
    }
}
