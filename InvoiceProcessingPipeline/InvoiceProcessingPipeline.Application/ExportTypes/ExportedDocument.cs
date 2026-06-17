using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.ExportTypes
{
    public sealed record ExportedDocument
    {
        public required byte[] Content { get; init; }

        public required string FileName { get; init; }

        public required string ContentType { get; init; }

        public required string Format { get; init; }

        public string? Encoding { get; init; }
    }
}
