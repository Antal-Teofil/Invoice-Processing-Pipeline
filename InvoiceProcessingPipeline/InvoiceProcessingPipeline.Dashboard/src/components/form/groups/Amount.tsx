import AmountFormSchema from "../../../schemas/invoice/amount.schema";
import { CURRENCY_CODE_OPTIONS } from "../../../shared/constants/currency-code.constant";
import { withFieldGroup } from "../setup/invoice-form";
import { amountFieldValidators } from "../../../validation/invoice";

export const AmountField = withFieldGroup({
  defaultValues: AmountFormSchema.parse({}),

  props: {
    title: "Amount",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="amount-section">
        <legend>{title}</legend>

        <group.AppField name="amount" validators={amountFieldValidators.amount}>
          {(field) => <field.NumberField label="Amount" />}
        </group.AppField>

        <group.AppField name="currencyCode">
          {(field) => (
            <field.OptionField
              label="Currency"
              options={CURRENCY_CODE_OPTIONS}
            />
          )}
        </group.AppField>
      </fieldset>
    );
  },
});