using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentCanonizer
    {
        public Task CanonizeExtractedDocumentData
    }
}
