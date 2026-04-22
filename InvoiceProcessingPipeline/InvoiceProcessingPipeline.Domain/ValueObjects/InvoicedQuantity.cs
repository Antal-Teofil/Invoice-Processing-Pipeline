using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record InvoicedQuantity(uint Quantity, string UnitCode): DocumentField;
}
