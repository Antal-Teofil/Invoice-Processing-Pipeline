import z from "zod";
import AmountSchema from "./amount.schema";
import TaxCategorySchema from "./tax-category.schema";
import { objectDefault } from "../../shared/utility/zod-default.utility";

const TaxSubtotalFormSchema = z.object({
  taxableAmount: objectDefault(AmountSchema),

  taxAmount: objectDefault(AmountSchema),

  taxCategory: objectDefault(TaxCategorySchema),
});

export default TaxSubtotalFormSchema;
