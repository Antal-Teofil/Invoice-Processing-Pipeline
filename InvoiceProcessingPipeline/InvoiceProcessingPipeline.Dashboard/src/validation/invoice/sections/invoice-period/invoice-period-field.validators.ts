import type { FieldValidatorArgs } from "../../shared/validator.types";
import { toDateTime } from "../../shared/value.utils";

function validateStartDate({ value, fieldApi }: FieldValidatorArgs) {
  const endDate = fieldApi.form.getFieldValue("endDate");
  const startDateTime = toDateTime(value);
  const endDateTime = toDateTime(endDate);

  if (startDateTime !== null && endDateTime !== null && startDateTime > endDateTime) {
    return "Start Date cannot be later than End Date.";
  }

  return undefined;
}

function validateEndDate({ value, fieldApi }: FieldValidatorArgs) {
  const startDate = fieldApi.form.getFieldValue("startDate");
  const startDateTime = toDateTime(startDate);
  const endDateTime = toDateTime(value);

  if (startDateTime !== null && endDateTime !== null && endDateTime < startDateTime) {
    return "End Date cannot be earlier than Start Date.";
  }

  return undefined;
}

export const invoicePeriodFieldValidators = {
  startDate: {
    onChangeListenTo: ["endDate"],
    onChange: validateStartDate,
    onSubmit: validateStartDate,
  },

  endDate: {
    onChangeListenTo: ["startDate"],
    onChange: validateEndDate,
    onSubmit: validateEndDate,
  },
} as const;
