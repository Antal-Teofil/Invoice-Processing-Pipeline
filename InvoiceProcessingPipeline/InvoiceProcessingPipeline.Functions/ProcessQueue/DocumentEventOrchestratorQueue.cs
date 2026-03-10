using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.ProcessQueue
{
    /*
     a queue-bol kivesszuk a DocumentIngestionEvent-et es elmentjuk az adatbazisba. Ez azert fontos, hogy az elejetol a vegeig tudjuk kovetni egy dokumentum feldolgozasanak eletciklusat,
    duplikaciokat ellenorzunk, mivel ugyanaz az event tobbszor is bejohet es nem akarjuk ugyanazt a dokumentumot ketszer feldolgozni.
    Innen inditjuk a feldolgozast. A sikeres schedule utan mentjuk majd adatbazisba a process id-t a megfelelo dokumentum metaadatokkal, hogy tudjuk kovetni a statuszvaltozast a frontend-rol. Maga a dokumentum statuszvaltozas az
    az audit feladata, ez csak arra kell hogy lehessen koordinalni az orchestraciot frontendrol (mikor kell feleleszteni a process-t, mikor kell varni kulso esemenyre, stb.). Tehat ezt csak folyamat koordinalasra van, nem auditalasra.
     */
    public sealed class DocumentEventOrchestratorQueue(ILogger<DocumentEventOrchestratorQueue> logger, IDocumentEventOrchestrator docOrchestrator)
    {
        [Function(nameof(DocumentEventOrchestratorQueue))]
        public async Task RunAsync([QueueTrigger("document-processing")] DocumentIngestionEvent docEvent)
        {
            await docOrchestrator.RecordEventAsync(docEvent);
            DocumentOrchestrationTaskID id = await docOrchestrator.StartDocumentOrchestrationAsync("DocumentIngestionOrchestrator");
            DocumentOrchestrationTask process = new(id, docEvent);
            await docOrchestrator.RecordDocumentOrchestrationEvent(process);
        }
    }
}
