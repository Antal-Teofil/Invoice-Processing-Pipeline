using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    // DueDate
    public sealed record DueDate(DateOnly Date): DocumentField;
}
