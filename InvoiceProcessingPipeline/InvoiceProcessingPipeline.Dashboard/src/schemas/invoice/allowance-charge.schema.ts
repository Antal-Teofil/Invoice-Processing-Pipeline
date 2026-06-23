import z from "zod";

import AmountSchema from "./amount.schema";
import TaxCategorySchema from "./tax-category.schema";
import { objectDefault } from "../../shared/utility/zod-default.utility";

const AllowanceChargeFormSchema = z.object({
  chargeIndicator: z.boolean().default(false),

  allowanceChargeReasonCode: z.string().nullable().default(null),

  allowanceChargeReason: z.string().nullable().default(null),

  multiplierFactorNumeric: z.number().nullable().default(null),

  amount: objectDefault(AmountSchema),

  baseAmount: objectDefault(AmountSchema),

  taxCategory: objectDefault(TaxCategorySchema),
});

export default AllowanceChargeFormSchema;
