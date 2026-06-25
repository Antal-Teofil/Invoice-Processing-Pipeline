import { PriceFormSchema } from "../../../schemas/invoice/price.schema";
import { withFieldGroup } from "../setup/invoice-form";

import { AmountField } from "./Amount";
import { QuantityField } from "./Quantity";
import { AllowanceCharge } from "./AllowanceCharge";

export const Price = withFieldGroup({
  defaultValues: PriceFormSchema.parse({}),

  props: {
    title: "Price",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="price-section">
        <legend>{title}</legend>

        <AmountField
          form={group}
          fields="priceAmount"
          title="Price Amount"
        />

        <QuantityField
          form={group}
          fields="baseQuantity"
          title="Base Quantity"
        />

        <AllowanceCharge
          form={group}
          fields="allowanceCharge"
          title="Allowance / Charge"
        />
      </fieldset>
    );
  },
});