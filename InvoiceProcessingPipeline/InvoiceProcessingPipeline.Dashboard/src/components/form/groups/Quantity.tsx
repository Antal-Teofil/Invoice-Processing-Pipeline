import { QuantityFormSchema } from "../../../schemas/invoice/quantity.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { quantityFieldValidators } from "../../../validation/invoice";
import { UNIT_CODE_OPTIONS } from "../../../shared/constants/unit-code.constant";

export const QuantityField = withFieldGroup({
  defaultValues: QuantityFormSchema.parse({}),

  props: {
    title: "Quantity",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="quantity-section">
        <legend>{title}</legend>

        <group.AppField name="amount" validators={quantityFieldValidators.amount}>
          {(field) => <field.NumberField label="Quantity" />}
        </group.AppField>

        <group.AppField name="unitCode" validators={quantityFieldValidators.unitCode}>
          {(field) => (
            <field.OptionField
              label="Unit Code"
              options={UNIT_CODE_OPTIONS}
            />
          )}
        </group.AppField>
      </fieldset>
    );
  },
});