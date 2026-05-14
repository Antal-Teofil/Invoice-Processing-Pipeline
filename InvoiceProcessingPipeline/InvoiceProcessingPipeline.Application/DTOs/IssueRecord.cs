using InvoiceProcessingPipeline.Application.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.DTOs
{
    public record IssueRecord
    {
        public required string Id { get; set; }
        // issues happened
        public required Accumulator Issues { get; init; }
        
        // date when the issue record was created
        public required DateTime RecordDate { get; init; }

        // invoice id
        public required string DocumentId { get; init; }
    }
}
