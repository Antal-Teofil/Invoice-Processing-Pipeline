using Castle.Core.Logging;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Application.Validations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public sealed class AnalyzeSchemaIntegrityActivity(ILogger<AnalyzeSchemaIntegrityActivity> loigger, IDocumentDataStore docStore)
    {
        [Function(nameof(AnalyzeSchemaIntegrityActivity))]
        public async Task<ActivityResult<Accumulator<SchemaViolation>>> RunAsync([ActivityTrigger] ExtractedDocumentResponse docResponse)
        {
            ExtractedDocumentData? exDoc = await docStore.RetrieveExtractedDocumentSchemaAsync(docResponse.ExtractedDocumentId);
            // I. beallitjuk a statuszt: Under Validation vagy valami hasonlora
            // II. validacios szakasz -> osszegyujtjuk az esetleges hibakat, vagy fast-fail bizonyos esetekben -> implementalasra var
            // III. amennyiben hiba lepett fel kuldjuk frontendre, ellenkezo esetben megy tovabb a validacios szakaszban.

            return ActivityResult<Accumulator<SchemaViolation>>.Failure("Schema Violation");
        }
    }
}
