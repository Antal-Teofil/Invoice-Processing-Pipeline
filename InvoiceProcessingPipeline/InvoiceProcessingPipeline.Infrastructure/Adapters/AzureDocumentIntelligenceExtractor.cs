using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using InvoiceProcessingPipeline.Domain.ValueObjects;


namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class AzureDocumentIntelligenceExtractor(
        DocumentIntelligenceClient client) : IDocumentDataExtractor
    {

        /// <summary>
        /// it is used for extracting information from a given document
        /// </summary>
        /// <param name="sasUri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ExtractedDocumentData> ExtractDocumentDataAsync(Uri sasUri, string processId ,CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(sasUri, nameof(sasUri));

            Operation<AnalyzeResult> result = await client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-invoice",
                sasUri,
                token);

            AnalyzeResult analyzedResult = result.Value;

            var document = analyzedResult.Documents.Count > 0
                            ? analyzedResult.Documents[0]
                            : throw new InvalidOperationException("The document is missing.");


            var builder = new ExtractedDocumentDataBuilder()
                .ExtractFieldAs("customerPartyName", () =>
                {
                    if (!document.Fields.TryGetValue("CustomerName", out var field) ||
                        field.FieldType != DocumentFieldType.String ||
                        string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        return new ExtractedDocumentField<PartyName>(
                            Extraction: new PartyName(string.Empty),
                            FieldName: "customerPartyName",
                            FieldOriginalContent: string.Empty,
                            ConfidenceScore: 0
                        );
                    }

                    return new ExtractedDocumentField<PartyName>(
                        Extraction: new PartyName(field.ValueString),
                        FieldName: "customerPartyName",
                        FieldOriginalContent: field.Content ?? string.Empty,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("customerPartyId", () =>
                {
                    if (!document.Fields.TryGetValue("CustomerTaxId", out var field) ||
                        field.FieldType != DocumentFieldType.String ||
                        string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        return new ExtractedDocumentField<PartyId>(
                            Extraction: new PartyId(string.Empty),
                            FieldName: "customerPartyId",
                            FieldOriginalContent: string.Empty,
                            ConfidenceScore: 0
                        );
                    }

                    return new ExtractedDocumentField<PartyId>(
                        Extraction: new PartyId(field.ValueString),
                        FieldName: "customerPartyId",
                        FieldOriginalContent: field.Content ?? string.Empty,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("invoiceId", () =>
                {
                    if (!document.Fields.TryGetValue("InvoiceId", out var field) ||
                        field.FieldType != DocumentFieldType.String ||
                        string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        return new ExtractedDocumentField<InvoiceNumber>(
                            Extraction: new InvoiceNumber(string.Empty),
                            FieldName: "invoiceId",
                            FieldOriginalContent: string.Empty,
                            ConfidenceScore: 0
                        );
                    }

                    return new ExtractedDocumentField<InvoiceNumber>(
                        Extraction: new InvoiceNumber(field.ValueString),
                        FieldName: "invoiceId",
                        FieldOriginalContent: field.Content ?? string.Empty,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("issueDate", () =>
                {
                    if (!document.Fields.TryGetValue("InvoiceDate", out var field) ||
                        field.FieldType != DocumentFieldType.Date ||
                        field.ValueDate is null)
                    {
                        return new ExtractedDocumentField<IssueDate>(
                            Extraction: new IssueDate(DateOnly.MinValue),
                            FieldName: "issueDate",
                            FieldOriginalContent: string.Empty,
                            ConfidenceScore: 0
                        );
                    }

                    return new ExtractedDocumentField<IssueDate>(
                        Extraction: new IssueDate(DateOnly.FromDateTime(field.ValueDate.Value.DateTime)),
                        FieldName: "issueDate",
                        FieldOriginalContent: field.Content ?? string.Empty,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("dueDate", () =>
                {
                    if (!document.Fields.TryGetValue("DueDate", out var field) ||
                        field.FieldType != DocumentFieldType.Date ||
                        field.ValueDate is null)
                    {
                        return new ExtractedDocumentField<DueDate>(
                            Extraction: new DueDate(DateOnly.MinValue),
                            FieldName: "dueDate",
                            FieldOriginalContent: string.Empty,
                            ConfidenceScore: 0
                        );
                    }

                    return new ExtractedDocumentField<DueDate>(
                        Extraction: new DueDate(DateOnly.FromDateTime(field.ValueDate.Value.DateTime)),
                        FieldName: "dueDate",
                        FieldOriginalContent: field.Content ?? string.Empty,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("vendorPartyName", () =>
                {
                    if (!document.Fields.TryGetValue("VendorName", out var field) ||
                        field.FieldType != DocumentFieldType.String ||
                        string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        return new ExtractedDocumentField<PartyName>(
                            Extraction: new PartyName(string.Empty),
                            FieldName: "vendorPartyName",
                            FieldOriginalContent: string.Empty,
                            ConfidenceScore: 0
                        );
                    }

                    return new ExtractedDocumentField<PartyName>(
                        Extraction: new PartyName(field.ValueString),
                        FieldName: "vendorPartyName",
                        FieldOriginalContent: field.Content ?? string.Empty,
                        ConfidenceScore: field.Confidence
                    );
                })
                .WithProcessId(processId);
            /*
                .ExtractFieldAs<PostalAddress>("vendorPostalAddress", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<PostalAddress>("customerPostalAddress", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<LineExtensionAmount>("legalMonetaryTotal.lineExtensionAmount", () => // SubTotal
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<AllowanceTotalAmount>("legalMonetaryTotal.allowanceTotalAmount", () => // TotalDiscount
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<TaxTotal>("taxTotal.taxAmount", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<TaxInclusiveAmount>("legalMonetaryTotal.taxInclusiveAmount", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<PayableAmount>("legalMonetaryTotal.payableAmount", () =>
                {
                    throw new NotImplementedException();
                });
            */
            // folytatjuk a a VendorTaxId-nal....
            return builder.Build();
        }


    }
}