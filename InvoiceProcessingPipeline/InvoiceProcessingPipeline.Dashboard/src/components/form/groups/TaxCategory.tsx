import TaxCategoryFormSchema from "../../../schemas/invoice/tax-category.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { taxCategoryFieldValidators } from "../../../validation/invoice";

export const TaxCategory = withFieldGroup({
  defaultValues: TaxCategoryFormSchema.parse({}),

  props: {
    title: "Tax Category",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="tax-category-section">
        <legend>{title}</legend>

        <group.AppField name="id" validators={taxCategoryFieldValidators.id}>
          {(field) => <field.TextField label="Tax Category ID" />}
        </group.AppField>

        <group.AppField name="percent" validators={taxCategoryFieldValidators.percent}>
          {(field) => <field.NumberField label="Tax Percent" />}
        </group.AppField>

        <group.AppField name="taxScheme" validators={taxCategoryFieldValidators.taxScheme}>
          {(field) => <field.TextField label="Tax Scheme" />}
        </group.AppField>
      </fieldset>
    );
  },
});