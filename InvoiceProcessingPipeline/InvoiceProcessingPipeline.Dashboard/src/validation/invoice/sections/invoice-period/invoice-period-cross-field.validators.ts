import type { FieldErrorBag } from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { hasValue, toDateTime } from "../../shared/value.utils";

type InvoicePeriodLike = {
  startDate?: unknown;
  endDate?: unknown;
};

export function validateInvoicePeriodCrossFields(
  period: InvoicePeriodLike | null | undefined,
  path: string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  if (!period) {
    return errors;
  }

  const hasStartDate = hasValue(period.startDate);
  const hasEndDate = hasValue(period.endDate);

  if (hasStartDate && !hasEndDate) {
    addFieldError(errors, `${path}.endDate`, "End Date is required when Start Date is set.");
  }

  if (!hasStartDate && hasEndDate) {
    addFieldError(errors, `${path}.startDate`, "Start Date is required when End Date is set.");
  }

  const startDateTime = toDateTime(period.startDate);
  const endDateTime = toDateTime(period.endDate);

  if (startDateTime !== null && endDateTime !== null && endDateTime < startDateTime) {
    addFieldError(errors, `${path}.endDate`, "End Date cannot be earlier than Start Date.");
  }

  return errors;
}
