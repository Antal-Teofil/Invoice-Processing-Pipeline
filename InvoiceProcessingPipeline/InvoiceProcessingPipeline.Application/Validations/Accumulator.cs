using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Validations
{
    public sealed record Accumulator<TSource> where TSource : class
    {
        public IReadOnlyList<TSource>? ConstraintViolations { get; set; }
    }
}
