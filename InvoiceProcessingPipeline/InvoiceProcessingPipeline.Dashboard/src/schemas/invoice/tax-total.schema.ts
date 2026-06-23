import z from "zod";
import AmountSchema from "./amount.schema";
import TaxSubtotalFormSchema from "./tax-subtotal.schema";
import {
  arrayDefault,
  objectDefault,
} from "../../shared/utility/zod-default.utility";

const TaxTotalFormSchema = z.object({
  taxAmount: objectDefault(AmountSchema),

  taxSubtotal: arrayDefault(TaxSubtotalFormSchema),
});
export default TaxTotalFormSchema;
