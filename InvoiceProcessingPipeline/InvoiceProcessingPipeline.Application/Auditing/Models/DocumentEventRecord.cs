using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Auditing.Models
{
    public sealed record DocumentEventRecord
    {
        public required string EventId { get; init; }
        public required string EventType { get; init; }
        public required string Source { get; init; }
        public required DateTimeOffset? EventTime { get; init; }
        public required string DocumentURL { get; init; }
        public required string ContentType { get; init; }
        public required string BlobType { get; init; }
        public long? ContentLength { get; init; }
        public required string ETag { get; init; }
    }
}
