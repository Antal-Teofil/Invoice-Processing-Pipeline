using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.DocumentAudit;
using InvoiceProcessingPipeline.Application.DTOs;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using Mapster;
using System.Net.Mail;

namespace InvoiceProcessingPipeline.Application.MapperConfigurations
{
    public sealed class ExtractedDocumentDataToDocumentRecordInformation : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ExtractedDocumentData, DocumentRecordInformation>()
                .Ignore(dest => dest.AuditStatus)
                .Map(dest => dest.ProcessId, src => src.ProcessId)
                .Map(dest => dest.InvoiceId, src => src.DocumentId)
                .Map(dest => dest.VendorName, src => GetVendorName(src))
                .Map(dest => dest.PhoneNumber, src => GetPhoneNumber(src))
                .Map(dest => dest.VendorEmailAddress, src => GetVendorEmailAddress(src))
                .Map(dest => dest.TotalAmount, src => GetTotalAmount(src))
                .Map(dest => dest.CurrencyCode, src => GetCurrencyCode(src));

            config.NewConfig<(ExtractedDocumentData Document, AuditStatus AuditStatus), DocumentRecordInformation>()
                .Map(dest => dest, src => src.Document)
                .Map(dest => dest.AuditStatus, src => src.AuditStatus);
        }

        private static string? GetVendorName(ExtractedDocumentData src) =>
            GetFieldValue<PartyName, string?>(src, "vendorPartyName", x => x.Name);

        private static string? GetPhoneNumber(ExtractedDocumentData src) =>
            GetFieldValue<PhoneNumber, string?>(src, "vendorPhoneNumber", x => x.Number);

        private static string? GetVendorEmailAddress(ExtractedDocumentData src) =>
            GetFieldValue<EmailAddress, string?>(src, "vendorEmailAddress", x => x.MailAddress);

        private static decimal? GetTotalAmount(ExtractedDocumentData src) =>
            GetFieldValue<TaxAmount, decimal?>(src, "totalAmount", x => x.Amount);

        private static char? GetCurrencyCode(ExtractedDocumentData src) =>
            GetFieldValue<CurrencyCode, char?>(src, "currencyCode", x => x.Code);

        private static TResult? GetFieldValue<TField, TResult>(
            ExtractedDocumentData src,
            string fieldName,
            Func<TField, TResult?> selector)
            where TField : DocumentField
        {
            if (!src.TryGetField<TField>(fieldName, out var field) || field is null)
            {
                return default;
            }

            return selector(field.Extraction);
        }
    }
}