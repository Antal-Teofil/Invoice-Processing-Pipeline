using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record InvoiceLine(string ID, Note? LineNote, InvoicedQuantity Quantity, LineExtensionAmount LEAmount, Item Kind, Price ItemPrice);
}
