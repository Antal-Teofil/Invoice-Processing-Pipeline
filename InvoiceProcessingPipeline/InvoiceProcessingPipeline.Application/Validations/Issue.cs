using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Validations
{
    public readonly record struct Issue(Severity Severity, Rule Code, string? Message, string? FieldName);
}
