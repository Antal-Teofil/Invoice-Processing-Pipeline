using InvoiceProcessingPipeline.Application.ExportTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentXmlStore
    {
        Task<Uri> StoreXmlDocumentSchemeAsync(ExportedDocument document, CancellationToken cancellationToken = default);
    }
}
