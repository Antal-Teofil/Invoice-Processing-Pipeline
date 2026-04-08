using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentDataSchema : AuditableEntity
    {
        public required Guid Id { get; set; }
        // 1..1
        // minden UBL szabvany szerinti dokumentumnak kotelezo modon kell lennie egy ilyennek, mert ez mondja meg, hogy melyik szabvany + melyik megszoritas rendszer
        public required CustomizationId CustomizationId { get; set; }
    }
}
