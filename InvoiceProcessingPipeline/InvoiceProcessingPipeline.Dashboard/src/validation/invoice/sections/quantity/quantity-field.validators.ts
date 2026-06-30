import { positiveNumber } from "../../shared/common-field.validators";
import { UNIT_CODES } from "../../../../shared/constants/unit-code.constant";

const validateQuantity = positiveNumber("Quantity");

function validateUnitCode({ value }: { value: string | null | undefined }) {
  if (!value) {
    return undefined;
  }

  if (!UNIT_CODES.includes(value)) {
    return "Unit Code must use one of the supported unit of measure codes available in the list.";
  }

  return undefined;
}

export const quantityFieldValidators = {
  amount: {
    onChange: validateQuantity,
    onSubmit: validateQuantity,
  },

  unitCode: {
    onChange: validateUnitCode,
    onSubmit: validateUnitCode,
  },
} as const;
