using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;


namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class AzureDocumentIntelligenceExtractor(
        DocumentIntelligenceClient client) : IDocumentDataExtractor
    {

        // fapados megoldas, ez betolti a teljes mindenseget
        public async Task<ExtractedDocumentData> ExtractDocumentDataAsync(Uri sasUri, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(sasUri, nameof(sasUri));

            Operation<AnalyzeResult> result = await client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-invoice",
                sasUri,
                token);

            AnalyzeResult analyzedResult = result.Value;

            var builder = new ExtractedDocumentDataBuilder()
                .ExtractFieldAs<PartyLegalEntity>("customerPartyLegalEntity", () =>
                {
                    throw new NotSupportedException();
                })
                .ExtractFieldAs<PartyName>("customerPartyName", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<PartyLegalEntity>("customerPartyId", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<InvoiceId>("invoiceId", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<IssueDate>("issueDate", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<DueDate>("dueDate", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<PartyName>("vendorPartyName", () =>
                {
                    throw new NotImplementedException();
                })
                .ExtractFieldAs<PartyLegalEntity>("vendorPartyLegalEntity", () =>
                {
                    throw new NotImplementedException();
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