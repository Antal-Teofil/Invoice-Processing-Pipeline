import type { CommercialInvoiceType } from "../types/form/invoice-form.types";
import { useAppForm } from "./form/setup/invoice-form";

import { AccountingParty } from "./form/groups/AccountingParty";
import { InvoiceHeader } from "./form/groups/InvoiceHeader";
import { InvoicePeriodFields } from "./form/groups/InvoicePeriod";
import { LegalMonetaryTotal } from "./form/groups/LegalMonetaryTotal";
import { TaxTotal } from "./form/groups/TaxTotal";
import { AllowanceCharge } from "./form/groups/AllowanceCharge";
import { InvoiceLine } from "./form/groups/InvoiceLine";

import TaxTotalFormSchema from "../schemas/invoice/tax-total.schema";
import AllowanceChargeFormSchema from "../schemas/invoice/allowance-charge.schema";
import InvoiceLineFormSchema from "../schemas/invoice/invoice-line.schema";

const emptyTaxTotal = TaxTotalFormSchema.parse({});
const emptyAllowanceCharge = AllowanceChargeFormSchema.parse({});
const emptyInvoiceLine = InvoiceLineFormSchema.parse({});

export function CommercialInvoiceEditorForm({
  invoice,
}: {
  invoice: CommercialInvoiceType;
}) {
  const form = useAppForm({
    defaultValues: invoice,

    onSubmit: ({ value }) => {
      console.log(value);
    },
  });

  return (
    <form
      onSubmit={(event) => {
        event.preventDefault();
        event.stopPropagation();
        form.handleSubmit();
      }}
    >
      <section>
        <h2>Invoice Details</h2>

        <InvoiceHeader form={form} />

        <InvoicePeriodFields
          form={form}
          fields="invoicePeriod"
          title="Invoice Period"
        />
      </section>

      <section>
        <h2>Parties</h2>

        <fieldset>
          <legend>Supplier Party</legend>

          <AccountingParty
            form={form}
            fields="accountingSupplierParty"
            title="Supplier Party"
          />
        </fieldset>

        <fieldset>
          <legend>Customer Party</legend>

          <AccountingParty
            form={form}
            fields="accountingCustomerParty"
            title="Customer Party"
          />
        </fieldset>
      </section>

      <section>
        <h2>Allowance / Charges</h2>

        <form.AppField name="allowanceCharge" mode="array">
          {(field) => {
            const allowanceCharges = field.state.value ?? [];

            return (
              <div>
                {allowanceCharges.map((_, index) => (
                  <fieldset key={index}>
                    <legend>Allowance / Charge {index + 1}</legend>

                    <AllowanceCharge
                      form={form}
                      fields={`allowanceCharge[${index}]`}
                      title={`Allowance / Charge ${index + 1}`}
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
                    field.pushValue(emptyAllowanceCharge);
                  }}
                >
                  Add
                </button>
              </div>
            );
          }}
        </form.AppField>
      </section>

      <section>
        <h2>Tax Totals</h2>

        <form.AppField name="taxTotal" mode="array">
          {(field) => {
            const taxTotals = field.state.value ?? [];

            return (
              <div>
                {taxTotals.map((_, index) => (
                  <fieldset key={index}>
                    <legend>Tax Total {index + 1}</legend>

                    <TaxTotal
                      form={form}
                      fields={`taxTotal[${index}]`}
                      title={`Tax Total ${index + 1}`}
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
                    field.pushValue(emptyTaxTotal);
                  }}
                >
                  Add
                </button>
              </div>
            );
          }}
        </form.AppField>
      </section>

      <section>
        <h2>Legal Monetary Total</h2>

        <LegalMonetaryTotal
          form={form}
          fields="legalMonetaryTotal"
          title="Legal Monetary Total"
        />
      </section>

      <section>
        <h2>Invoice Lines</h2>

        <form.AppField name="invoiceLine" mode="array">
          {(field) => {
            const invoiceLines = field.state.value ?? [];

            return (
              <div>
                {invoiceLines.map((_, index) => (
                  <fieldset key={index}>
                    <legend>Invoice Line {index + 1}</legend>

                    <InvoiceLine
                      form={form}
                      fields={`invoiceLine[${index}]`}
                      title={`Invoice Line ${index + 1}`}
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
                    field.pushValue(emptyInvoiceLine);
                  }}
                >
                  Add
                </button>
              </div>
            );
          }}
        </form.AppField>
      </section>

      <footer>
        <button type="submit">Save</button>
      </footer>
    </form>
  );
}
