using InvoiceProcessingPipeline.Application.ExportTypes;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentSchemeExporter
    {
        Task<ExportedDocument> ExportAsync(CommercialInvoice invoice, CancellationToken cancellationToken = default);
    }
}
