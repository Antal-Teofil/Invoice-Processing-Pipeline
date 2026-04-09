using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Shared
{
    public sealed class PagedResult<TSource> where TSource : class
    {
        public IReadOnlyList<TSource> Items { get; init; } = [];
        public string? ContinuationToken { get; init; }
        public bool HasMore => !string.IsNullOrEmpty(ContinuationToken);
    }
}
