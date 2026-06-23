import z from "zod";
import { QuantityFormSchema } from "./quantity.schema";
import AllowanceChargeFormSchema from "./allowance-charge.schema";
import AmountFormSchema from "./amount.schema";
import { objectDefault } from "../../shared/utility/zod-default.utility";

export const PriceFormSchema = z.object({
  priceAmount: objectDefault(AmountFormSchema),
  baseQuantity: objectDefault(QuantityFormSchema),
  allowanceCharge: objectDefault(AllowanceChargeFormSchema),
});
