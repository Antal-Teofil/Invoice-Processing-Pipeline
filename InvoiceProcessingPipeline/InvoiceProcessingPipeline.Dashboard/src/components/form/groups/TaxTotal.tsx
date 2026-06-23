import TaxTotalFormSchema from "../../../schemas/invoice/tax-total.schema";
import TaxSubtotalFormSchema from "../../../schemas/invoice/tax-subtotal.schema";

import { withFieldGroup } from "../setup/invoice-form";
import { AmountField } from "./Amount";
import { TaxSubtotal } from "./TaxSubtotal";

const emptyTaxSubtotal = TaxSubtotalFormSchema.parse({});

export const TaxTotal = withFieldGroup({
  defaultValues: TaxTotalFormSchema.parse({}),

  props: {
    title: "Tax Total",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <AmountField
          form={group}
          fields="taxAmount"
          title="Tax Amount"
        />

        <group.AppField name="taxSubtotal" mode="array">
          {(field) => {
            const subtotals = field.state.value ?? [];

            return (
              <fieldset>
                <legend>Tax Subtotals</legend>

                {subtotals.map((_, index) => (
                  <fieldset key={index}>
                    <TaxSubtotal
                      form={group}
                      fields={`taxSubtotal[${index}]`}
                      title={`Tax Subtotal ${index + 1}`}
                    />

                    <button
                      type="button"
                      onClick={() => {
                        field.removeValue(index);
                      }}
                    >
                      Remove
                    </button>
                  </fieldset>
                ))}

                <button
                  type="button"
                  onClick={() => {
                    field.pushValue(emptyTaxSubtotal);
                  }}
                >
                  Add
                </button>
              </fieldset>
            );
          }}
        </group.AppField>
      </fieldset>
    );
  },
});