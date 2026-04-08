using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Shared
{
    public sealed record AuditResult<T> where T : class
    {
        public T? Value { get; init; }
    }
}
