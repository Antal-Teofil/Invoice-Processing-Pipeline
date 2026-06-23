import { CommercialInvoiceFormSchema } from "../../../schemas/invoice/invoice.schema";
import { CURRENCY_CODE_OPTIONS } from "../../../shared/constants/currency-code.constant";
import { INVOICE_TYPE_OPTIONS } from "../../../shared/constants/invoice-type-code.constant";
import { withForm } from "../setup/invoice-form";

export const InvoiceHeader = withForm({
  defaultValues: CommercialInvoiceFormSchema.parse({}),

  render: function Render({ form }) {
    return (
      <fieldset>
        <legend>Invoice Header</legend>

        <form.AppField name="invoiceNumber">
          {(field) => <field.TextField label="Invoice Number" />}
        </form.AppField>

        <form.AppField name="issueDate">
          {(field) => <field.DateField label="Issue Date" />}
        </form.AppField>

        <form.AppField name="dueDate">
          {(field) => <field.DateField label="Due Date" />}
        </form.AppField>

        <form.AppField name="typeCode">
          {(field) => (
            <field.OptionField
              label="Invoice Type Code"
              options={INVOICE_TYPE_OPTIONS}
            />
          )}
        </form.AppField>

        <form.AppField name="taxPointDate">
          {(field) => <field.DateField label="Tax Point Date" />}
        </form.AppField>

        <form.AppField name="documentCurrencyCode">
          {(field) => (
            <field.OptionField
              label="Document Currency Code"
              options={CURRENCY_CODE_OPTIONS}
            />
          )}
        </form.AppField>

        <form.AppField name="taxCurrencyCode">
          {(field) => (
            <field.OptionField
              label="Tax Currency Code"
              options={CURRENCY_CODE_OPTIONS}
            />
          )}
        </form.AppField>

        <form.AppField name="note">
          {(field) => <field.TextField label="Invoice Note" />}
        </form.AppField>
      </fieldset>
    );
  },
});