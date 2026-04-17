using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    // ez a csoda kiszedi es el is menti a kinyert adatokat mindenestul, s beleteszi az adatbazisba. Doksi szerint nem ajanlott nagy payload-u anyagot visszaadni activitybe, mert a bemenet es a kimenet automatikusan mentodik a storage-ba es oda pici infok kellenek
    // tehat az lesz hogy csak az id-t s esetleg valami plusz hasnzops infot teszek bele s majd a kovetkezo activity kikeri adatbazisbol s kanonizalja.
    /*
     Tehat mit csinal ez?

    1. extract
    2. nyersen menti a szukseges adatokat
     */

    // itt majd nem csak user delegation sas urit fogok atadni hanem korrelacio kulcsot is csak most egyelore meg nem bonyolitjuk
    public class ExtractDocumentDataActivity(ILogger<ExtractDocumentDataActivity> logger, IDocumentDataExtractor extractor, IDocumentDataStore documentDataStore)
    {
        [Function(nameof(ExtractDocumentDataActivity))]
        public async Task<ActivityResult<ExtractedDocumentResponse>> RunAsync([ActivityTrigger] DocumentUserDelegationSasUri sasUri, CancellationToken token)
        {
            Uri userDelegationSasUri = sasUri.SasUri;

            if(userDelegationSasUri is null)
            {
                return ActivityResult<ExtractedDocumentResponse>.Failure("User Delegation SAS URI must be a non-null value");
            }

            ExtractedDocumentData extractedDocumentData = await extractor.ExtractDocumentDataAsync(userDelegationSasUri, token);

            logger.LogInformation("Document extraction occurred with id: {Id}", extractedDocumentData.DocumentId);

            await documentDataStore.StoreExtractedDocumentSchemaAsync(extractedDocumentData);

            logger.LogInformation("Extrcated document with id: {Id} was saved successfully", extractedDocumentData.DocumentId);

            return ActivityResult<ExtractedDocumentResponse>.Success(new ExtractedDocumentResponse { ExtractedDocumentId = extractedDocumentData.DocumentId});
        }
    }
}
