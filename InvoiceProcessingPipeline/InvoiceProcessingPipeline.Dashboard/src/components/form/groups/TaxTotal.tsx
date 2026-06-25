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
      <fieldset className="tax-total-section">
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
              <fieldset className="tax-subtotal-array-section">
                <legend>Tax Subtotals</legend>

                {subtotals.map((_, index) => (
                  <fieldset key={index} className="tax-subtotal-array-item">
                    <TaxSubtotal
                      form={group}
                      fields={`taxSubtotal[${index}]`}
                      title={`Tax Subtotal ${index + 1}`}
                    />

                    <div className="form-array-actions">
                      <button
                        type="button"
                        className="form-button form-button-danger"
                        onClick={() => {
                          field.removeValue(index);
                        }}
                      >
                        Remove
                      </button>
                    </div>
                  </fieldset>
                ))}

                <div className="form-array-actions">
                  <button
                    type="button"
                    className="form-button form-button-primary"
                    onClick={() => {
                      field.pushValue(emptyTaxSubtotal);
                    }}
                  >
                    Add
                  </button>
                </div>
              </fieldset>
            );
          }}
        </group.AppField>
      </fieldset>
    );
  },
});