using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record InvoicePeriod(DateOnly? StartDate, DateOnly? EndDate, string? DescriptionCode) : DocumentField;
}
