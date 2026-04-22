using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record AdditionalItemProperty(string Name, string Value) : DocumentField;
}
