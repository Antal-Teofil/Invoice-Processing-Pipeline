import InvoiceLineFormSchema from "../../../schemas/invoice/invoice-line.schema";
import AllowanceChargeFormSchema from "../../../schemas/invoice/allowance-charge.schema";

import { withFieldGroup } from "../setup/invoice-form";

import { QuantityField } from "./Quantity";
import { AmountField } from "./Amount";
import { InvoicePeriodFields } from "./InvoicePeriod";
import { AllowanceCharge } from "./AllowanceCharge";
import { Item } from "./Item";
import { Price } from "./Price";

const emptyAllowanceCharge = AllowanceChargeFormSchema.parse({});

export const InvoiceLine = withFieldGroup({
  defaultValues: InvoiceLineFormSchema.parse({}),

  props: {
    title: "Invoice Line",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="lineId">
          {(field) => <field.TextField label="Line ID" />}
        </group.AppField>

        <group.AppField name="note">
          {(field) => <field.TextField label="Note" />}
        </group.AppField>

        <QuantityField
          form={group}
          fields="invoicedQuantity"
          title="Invoiced Quantity"
        />

        <AmountField
          form={group}
          fields="lineExtensionAmount"
          title="Line Extension Amount"
        />

        <InvoicePeriodFields
          form={group}
          fields="invoicePeriod"
          title="Invoice Period"
        />

        <group.AppField name="allowanceCharge" mode="array">
          {(field) => {
            const allowanceCharges = field.state.value ?? [];

            return (
              <fieldset>
                <legend>Allowance / Charges</legend>

                {allowanceCharges.map((_, index) => (
                  <fieldset key={index}>
                    <AllowanceCharge
                      form={group}
                      fields={`allowanceCharge[${index}]`}
                      title={`Allowance / Charge ${index + 1}`}
                    />

                    <button
                      type="button"
                      onClick={() => {
                        field.removeValue(index);
                      }}
                    >
                      Remove allowance / charge
                    </button>
                  </fieldset>
                ))}

                <button
                  type="button"
                  onClick={() => {
                    field.pushValue(emptyAllowanceCharge);
                  }}
                >
                  Add allowance / charge
                </button>
              </fieldset>
            );
          }}
        </group.AppField>

        <Item form={group} fields="item" title="Item" />

        <Price form={group} fields="price" title="Price" />
      </fieldset>
    );
  },
});