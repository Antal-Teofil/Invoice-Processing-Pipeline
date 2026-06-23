import z from "zod";
import AmountFormSchema from "./amount.schema";
import { objectDefault } from "../../shared/utility/zod-default.utility";

const LegalMonetaryTotalFormSchema = z.object({
  lineExtensionAmount: objectDefault(AmountFormSchema),

  taxExclusiveAmount: objectDefault(AmountFormSchema),

  allowanceTotalAmount: objectDefault(AmountFormSchema),

  chargeTotalAmount: objectDefault(AmountFormSchema),

  prepaidAmount: objectDefault(AmountFormSchema),

  payableRoundingAmount: objectDefault(AmountFormSchema),

  payableAmount: objectDefault(AmountFormSchema),
});

export default LegalMonetaryTotalFormSchema;
