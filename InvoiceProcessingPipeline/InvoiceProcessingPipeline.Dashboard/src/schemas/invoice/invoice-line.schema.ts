import z from "zod";
import AmountSchema from "./amount.schema";
import InvoicePeriodSchema from "./invoice-period.shema";
import AllowanceChargeSchema from "./allowance-charge.schema";
import { QuantityFormSchema } from "./quantity.schema";
import { ItemFormSchema } from "./item.schema";
import { PriceFormSchema } from "./price.schema";
import {
  arrayDefault,
  objectDefault,
} from "../../shared/utility/zod-default.utility";

const InvoiceLineFormSchema = z.object({
  lineId: z.string().nullable().default(null),

  note: z.string().nullable().default(null),

  invoicedQuantity: objectDefault(QuantityFormSchema),

  lineExtensionAmount: objectDefault(AmountSchema),

  invoicePeriod: objectDefault(InvoicePeriodSchema),

  allowanceCharge: arrayDefault(AllowanceChargeSchema),

  item: objectDefault(ItemFormSchema),

  price: objectDefault(PriceFormSchema),
});
export default InvoiceLineFormSchema;
