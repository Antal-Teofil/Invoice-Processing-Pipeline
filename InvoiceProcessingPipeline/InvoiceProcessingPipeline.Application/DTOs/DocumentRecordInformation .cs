using InvoiceProcessingPipeline.Application.DocumentAudit;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.DTOs
{
    public sealed record DocumentRecordInformation
    {
        public required AuditStatus AuditStatus { get; set; }
        public required string ProcessId { get; set; }
        public string? InvoiceId { get; set; }
        public string? VendorName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? VendorEmailAddress { get; set; }
        public decimal? TotalAmount { get; set; } = 1000;
        public char? CurrencyCode { get; set; } = '$';
    }
}
