using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using System.Net;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    /// <summary>
    /// this wonder is responsible for schema validation
    /// we need an instanceId, so we can identify an instance which is attached to a given document, we use this instanceId in order to manipulate the workflow
    /// </summary>
    public sealed class ApplyConstraintIntegrityCorrectionTrigger
    {
        [Function(nameof(ApplyConstraintIntegrityCorrectionTrigger))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "PATCH", Route = "invoice/validation/{instanceId}")] HttpRequestData request, [DurableClient] DurableTaskClient durableClient , string instanceId)
        {
            var payload = request.ReadFromJsonAsync<DocumentCorrectionSubmitted>();

            // amennyiben a kikuldott dokumentum korrekciora var es a korrekcio tovabbitasra kerult a backend fele, akkor folytatjuk a process-t
            /* korrekcios fazis eseten ket eset merulhet fel:
               1. a korrekcio nem teljes, igy adott esetben szukseges ujra elvegezni ugyanazt a korrekcios fazist
               2. a korrekcio sikeres, vagyis kovetkezhet az uzleti logikai validacios szakasz
            */
            
            await durableClient.RaiseEventAsync(instanceId, nameof(DocumentCorrectionSubmitted), payload);
            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
