import TaxCategoryFormSchema from "../../../schemas/invoice/tax-category.schema";
import { withFieldGroup } from "../setup/invoice-form";

export const TaxCategory = withFieldGroup({
  defaultValues: TaxCategoryFormSchema.parse({}),

  props: {
    title: "Tax Category",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="id">
          {(field) => <field.TextField label="Tax Category ID" />}
        </group.AppField>

        <group.AppField name="percent">
          {(field) => <field.NumberField label="Tax Percent" />}
        </group.AppField>

        <group.AppField name="taxScheme">
          {(field) => <field.TextField label="Tax Scheme" />}
        </group.AppField>
      </fieldset>
    );
  },
});