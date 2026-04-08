using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    // InvoiceId (AADI)
    public sealed record InvoiceId(string ID) : DocumentField;
}
