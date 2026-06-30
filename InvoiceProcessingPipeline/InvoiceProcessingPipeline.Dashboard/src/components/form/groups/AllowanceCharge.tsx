import AllowanceChargeFormSchema from "../../../schemas/invoice/allowance-charge.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { allowanceChargeFieldValidators } from "../../../validation/invoice";

import { AmountField } from "./Amount";
import { TaxCategory } from "./TaxCategory";

export const AllowanceCharge = withFieldGroup({
  defaultValues: AllowanceChargeFormSchema.parse({}),

  props: {
    title: "Allowance / Charge",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="allowance-charge-section">
        <legend>{title}</legend>

        <group.AppField name="chargeIndicator">
          {(field) => <field.CheckboxField label="Charge Indicator" />}
        </group.AppField>

        <group.AppField name="allowanceChargeReasonCode">
          {(field) => (
            <field.TextField label="Allowance / Charge Reason Code" />
          )}
        </group.AppField>

        <group.AppField name="allowanceChargeReason">
          {(field) => (
            <field.TextField label="Allowance / Charge Reason" />
          )}
        </group.AppField>

        <group.AppField name="multiplierFactorNumeric" validators={allowanceChargeFieldValidators.multiplierFactorNumeric}>
          {(field) => (
            <field.NumberField label="Multiplier Factor Numeric" />
          )}
        </group.AppField>

        <AmountField
          form={group}
          fields="amount"
          title="Amount"
        />

        <AmountField
          form={group}
          fields="baseAmount"
          title="Base Amount"
        />

        <TaxCategory
          form={group}
          fields="taxCategory"
          title="Tax Category"
        />
      </fieldset>
    );
  },
});