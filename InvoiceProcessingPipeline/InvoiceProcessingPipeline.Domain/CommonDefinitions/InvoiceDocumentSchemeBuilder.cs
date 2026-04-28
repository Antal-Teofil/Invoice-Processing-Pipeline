using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class InvoiceDocumentSchemeBuilder : IDocumentSchemeBuilder<InvoiceDocumentScheme>
    {
        public abstract InvoiceDocumentScheme Build();
    }
}
