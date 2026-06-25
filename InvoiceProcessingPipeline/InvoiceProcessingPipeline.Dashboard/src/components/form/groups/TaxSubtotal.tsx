import TaxSubtotalFormSchema from "../../../schemas/invoice/tax-subtotal.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { AmountField } from "./Amount";
import { TaxCategory } from "./TaxCategory";

export const TaxSubtotal = withFieldGroup({
  defaultValues: TaxSubtotalFormSchema.parse({}),

  props: {
    title: "Tax Subtotal",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="tax-subtotal-section">
        <legend>{title}</legend>

        <AmountField
          form={group}
          fields="taxableAmount"
          title="Taxable Amount"
        />

        <AmountField
          form={group}
          fields="taxAmount"
          title="Tax Amount"
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