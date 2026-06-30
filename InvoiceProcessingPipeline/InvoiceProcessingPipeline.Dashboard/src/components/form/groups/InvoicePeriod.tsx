import InvoicePeriodFormSchema from "../../../schemas/invoice/invoice-period.shema";
import { VAT_DATE_CODE_OPTIONS } from "../../../shared/constants/vat-date-code.constant";
import { withFieldGroup } from "../setup/invoice-form";
import { invoicePeriodFieldValidators } from "../../../validation/invoice";

export const InvoicePeriodFields = withFieldGroup({
  defaultValues: InvoicePeriodFormSchema.parse({}),

  props: {
    title: "Invoice Period",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="invoice-period-section">
        <legend>{title}</legend>

        <group.AppField name="startDate" validators={invoicePeriodFieldValidators.startDate}>
          {(field) => <field.DateField label="Start Date" />}
        </group.AppField>

        <group.AppField name="endDate" validators={invoicePeriodFieldValidators.endDate}>
          {(field) => <field.DateField label="End Date" />}
        </group.AppField>

        <group.AppField name="descriptionCode">
          {(field) => (
            <field.OptionField
              label="VAT date code"
              options={VAT_DATE_CODE_OPTIONS}
            />
          )}
        </group.AppField>
      </fieldset>
    );
  },
});