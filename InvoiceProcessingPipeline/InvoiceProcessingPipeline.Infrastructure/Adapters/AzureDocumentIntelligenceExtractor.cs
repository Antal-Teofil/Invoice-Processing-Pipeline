using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;  
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
        public async Task<ExtractedDocumentData> ExtractDocumentDataAsync(Uri sasUri, CancellationToken token)
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
                .ExtractFieldAs("customerPartyRegistrationName", () =>
                {
                    if (!document.Fields.TryGetValue("CustomerName", out var field))
                    {
                        // később ezt lehet warning mechanizmusra cserélni, nem akarunk exceptiont dobni ebben a fazisban
                        throw new InvalidOperationException("Missing CustomerName");
                    }

                    if (field.FieldType != DocumentFieldType.String || string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        throw new InvalidOperationException("CustomerName is not a valid string field");
                    }

                    return new ExtractedDocumentField<PartyRegistrationName>(
                        Extraction: new PartyRegistrationName(field.ValueString),
                        FieldName: "customerPartyRegistrationName",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("customerPartyName", () =>
                {

                    if (!document.Fields.TryGetValue("CustomerName", out var field))
                    {
                        // később ezt lehet warning mechanizmusra cserélni
                        throw new InvalidOperationException("Missing CustomerName");
                    }

                    if (field.FieldType != DocumentFieldType.String || string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        throw new InvalidOperationException("CustomerName is not a valid string field");
                    }

                    return new ExtractedDocumentField<PartyName>(
                        Extraction: new PartyName(field.ValueString),
                        FieldName: "customerPartyRegistrationName",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("customerPartyId", () =>
                {
                    if (!document.Fields.TryGetValue("CustomerId", out var field))
                    {
                        throw new InvalidOperationException("Missing CustomerName");
                    }

                    if (field.FieldType != DocumentFieldType.String || string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        throw new InvalidOperationException("CustomerId is not a valid string field");
                    }

                    return new ExtractedDocumentField<PartyId>(
                        Extraction: new PartyId(field.ValueString),
                        FieldName: "customerPartyId",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("invoiceId", () =>
                {
                    if (!document.Fields.TryGetValue("InvoiceId", out var field))
                    {
                        throw new InvalidOperationException("Missing invoice id");
                    }

                    if (field.FieldType != DocumentFieldType.String || string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        throw new InvalidOperationException("InvoiceId is not a valid string field");
                    }

                    return new ExtractedDocumentField<InvoiceId>(
                        Extraction: new InvoiceId(field.ValueString),
                        FieldName: "customerPartyId",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("issueDate", () =>
                {
                    if (!document.Fields.TryGetValue("InvoiceDate", out var field))
                    {
                        throw new InvalidOperationException("Missing InvoiceDate");
                    }

                    if (field.FieldType != DocumentFieldType.Date || field.ValueDate is null)
                    {
                        throw new InvalidOperationException("InvoiceDate is not a valid date field");
                    }

                    return new ExtractedDocumentField<IssueDate>(
                        Extraction: new IssueDate(field.ValueDate.Value),
                        FieldName: "issueDate",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("dueDate", () =>
                {
                    if (!document.Fields.TryGetValue("DueDate", out var field))
                    {
                        throw new InvalidOperationException("Missing DueDate");
                    }

                    if (field.FieldType != DocumentFieldType.Date || field.ValueDate is null)
                    {
                        throw new InvalidOperationException("DueDate is not a valid date field");
                    }

                    return new ExtractedDocumentField<DueDate>(
                        Extraction: new DueDate(field.ValueDate.Value),
                        FieldName: "dueDate",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("vendorPartyName", () =>
                {
                    if (!document.Fields.TryGetValue("VendorName", out var field))
                    {
                        // később ezt lehet warning mechanizmusra cserélni
                        throw new InvalidOperationException("Missing VendorName");
                    }

                    if (field.FieldType != DocumentFieldType.String || string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        throw new InvalidOperationException("VendorName is not a valid string field");
                    }

                    return new ExtractedDocumentField<PartyName>(
                        Extraction: new PartyName(field.ValueString),
                        FieldName: "vendorPartyName",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
                .ExtractFieldAs("vendorPartyRegistrationName", () =>
                {
                    if (!document.Fields.TryGetValue("VendorName", out var field))
                    {
                        // később ezt lehet warning mechanizmusra cserélni
                        throw new InvalidOperationException("Missing VendorName");
                    }

                    if (field.FieldType != DocumentFieldType.String || string.IsNullOrWhiteSpace(field.ValueString))
                    {
                        throw new InvalidOperationException("VendorName is not a valid string field");
                    }

                    return new ExtractedDocumentField<PartyRegistrationName>(
                        Extraction: new PartyRegistrationName(field.ValueString),
                        FieldName: "vendorPartyRegistrationName",
                        FieldOriginalContent: field.Content,
                        ConfidenceScore: field.Confidence
                    );
                })
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
                }).Build();

                // folytatjuk a a VendorTaxId-nal....
            return null!;
        }


    }
}