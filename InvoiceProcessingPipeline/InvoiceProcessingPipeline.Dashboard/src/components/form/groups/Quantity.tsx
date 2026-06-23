import { QuantityFormSchema } from "../../../schemas/invoice/quantity.schema";
import { withFieldGroup } from "../setup/invoice-form";

export const QuantityField = withFieldGroup({
  defaultValues: QuantityFormSchema.parse({}),

  props: {
    title: "Quantity",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="amount">
          {(field) => <field.NumberField label="Quantity" />}
        </group.AppField>

        <group.AppField name="unitCode">
          {(field) => <field.TextField label="Unit Code" />}
        </group.AppField>
      </fieldset>
    );
  },
});