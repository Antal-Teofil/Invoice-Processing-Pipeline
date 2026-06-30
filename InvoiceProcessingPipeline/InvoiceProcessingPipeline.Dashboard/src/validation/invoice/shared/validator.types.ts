import type { z } from "zod";
import { CommercialInvoiceFormSchema } from "../../../schemas/invoice/invoice.schema";

export type CommercialInvoiceValidationModel = z.infer<typeof CommercialInvoiceFormSchema>;

export type FieldValidatorArgs = {
  value: unknown;
  fieldApi: {
    form: {
      getFieldValue: (field: string) => unknown;
      state: {
        values: unknown;
      };
    };
  };
};

export type FieldValidators = Record<string, unknown>;

export type FieldErrorBag = Record<string, string>;

export type TanStackFormErrorResult =
  | {
      fields: FieldErrorBag;
    }
  | undefined;
