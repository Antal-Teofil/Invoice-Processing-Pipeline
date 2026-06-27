using InvoiceProcessingPipeline.Application.DocumentAudit;
using InvoiceProcessingPipeline.Application.DTOs.InvoiceDTOs;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations
{
    public sealed class InvoiceSchemeToInvoiceDto : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CommercialInvoice, CommercialInvoiceSchemaDto>()
                .Map(dest => dest.Metadata.DocumentId, src => src.DocumentId)
                .Map(dest => dest.Metadata.WorkflowId, src => src.WorkflowId)
                .Map(dest => dest.Metadata.AuditStatus, src => Enum.Parse<AuditStatus>(src.AuditStatus))
                .Map(dest => dest.Metadata.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.Content.InvoiceNumber, src => src.InvoiceId.Value)
                .IgnoreNullValues(true);
        }
    }
}
