using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentDataSchema : AuditableEntity
    {
        // 1..1
        // minden UBL szabvany szerinti dokumentumnak kotelezo modon kell lennie egy ilyennek, mert ez mondja meg, hogy melyik szabvany + melyik megszoritas rendszer
        public required CustomizationId CustomizationId { get; set; }
    }
}
