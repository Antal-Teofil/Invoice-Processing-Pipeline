import z from "zod";
import { COUNTRY_CODES } from "../../shared/constants/country-code.constant";
import { AdditionalItemPropertyFormSchema } from "./additional-item-prop.schema";
import TaxCategoryFormSchema from "./tax-category.schema";
import {
  arrayDefault,
  objectDefault,
} from "../../shared/utility/zod-default.utility";

export const ItemFormSchema = z.object({
  description: z.string().nullable().default(null),

  name: z.string().nullable().default(null),

  countryOriginCode: z.enum(COUNTRY_CODES).nullable().default(null),

  classifiedTaxCategory: objectDefault(TaxCategoryFormSchema),

  additionalItemProperty: arrayDefault(AdditionalItemPropertyFormSchema),
});
