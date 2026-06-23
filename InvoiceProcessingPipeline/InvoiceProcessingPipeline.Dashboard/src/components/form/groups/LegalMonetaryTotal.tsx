import LegalMonetaryTotalFormSchema from "../../../schemas/invoice/legal-monetary-total.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { AmountField } from "./Amount";

export const LegalMonetaryTotal = withFieldGroup({
  defaultValues: LegalMonetaryTotalFormSchema.parse({}),

  props: {
    title: "Legal Monetary Total",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <AmountField
          form={group}
          fields="lineExtensionAmount"
          title="Line Extension Amount"
        />

        <AmountField
          form={group}
          fields="taxExclusiveAmount"
          title="Tax Exclusive Amount"
        />

        <AmountField
          form={group}
          fields="allowanceTotalAmount"
          title="Allowance Total Amount"
        />

        <AmountField
          form={group}
          fields="chargeTotalAmount"
          title="Charge Total Amount"
        />

        <AmountField
          form={group}
          fields="prepaidAmount"
          title="Prepaid Amount"
        />

        <AmountField
          form={group}
          fields="payableRoundingAmount"
          title="Payable Rounding Amount"
        />

        <AmountField
          form={group}
          fields="payableAmount"
          title="Payable Amount"
        />
      </fieldset>
    );
  },
});