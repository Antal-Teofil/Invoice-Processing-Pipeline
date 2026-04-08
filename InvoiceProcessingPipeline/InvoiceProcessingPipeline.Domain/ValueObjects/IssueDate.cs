using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    // InvoiceDate (AADI)
    public sealed record IssueDate(DateTimeOffset Date) : DocumentField;
}
