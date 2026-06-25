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
      console.log(JSON.stringify(value, null, 2));
    },
  });

  return (
    <form
      className="invoice-editor-form"
      onSubmit={(event) => {
        event.preventDefault();
        event.stopPropagation();
        form.handleSubmit();
      }}
    >
      <section className="invoice-editor-section">
        <h2 className="invoice-editor-section-title">Invoice Details</h2>

        <InvoiceHeader form={form} />

        <InvoicePeriodFields
          form={form}
          fields="invoicePeriod"
          title="Invoice Period"
        />
      </section>

      <section className="invoice-editor-section">
        <h2 className="invoice-editor-section-title">Parties</h2>

        <AccountingParty
          form={form}
          fields="accountingSupplierParty"
          title="Supplier Party"
        />

        <AccountingParty
          form={form}
          fields="accountingCustomerParty"
          title="Customer Party"
        />
      </section>

      <section className="invoice-editor-section">
        <h2 className="invoice-editor-section-title">Allowance / Charges</h2>

        <form.AppField name="allowanceCharge" mode="array">
          {(field) => {
            const allowanceCharges = field.state.value ?? [];

            return (
              <fieldset className="form-array-section">
                <legend>Allowance / Charges</legend>

                {allowanceCharges.map((_, index) => (
                  <div key={index} className="form-array-item-flat">
                    <AllowanceCharge
                      form={form}
                      fields={`allowanceCharge[${index}]`}
                      title={`Allowance / Charge ${index + 1}`}
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
                  </div>
                ))}

                <div className="form-array-actions">
                  <button
                    type="button"
                    className="form-button form-button-primary"
                    onClick={() => {
                      field.pushValue(emptyAllowanceCharge);
                    }}
                  >
                    Add
                  </button>
                </div>
              </fieldset>
            );
          }}
        </form.AppField>
      </section>

      <section className="invoice-editor-section">
        <h2 className="invoice-editor-section-title">Tax Totals</h2>

        <form.AppField name="taxTotal" mode="array">
          {(field) => {
            const taxTotals = field.state.value ?? [];

            return (
              <fieldset className="form-array-section">
                <legend>Tax Totals</legend>

                {taxTotals.map((_, index) => (
                  <div key={index} className="form-array-item-flat">
                    <TaxTotal
                      form={form}
                      fields={`taxTotal[${index}]`}
                      title={`Tax Total ${index + 1}`}
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
                  </div>
                ))}

                <div className="form-array-actions">
                  <button
                    type="button"
                    className="form-button form-button-primary"
                    onClick={() => {
                      field.pushValue(emptyTaxTotal);
                    }}
                  >
                    Add
                  </button>
                </div>
              </fieldset>
            );
          }}
        </form.AppField>
      </section>

      <section className="invoice-editor-section">
        <h2 className="invoice-editor-section-title">Legal Monetary Total</h2>

        <LegalMonetaryTotal
          form={form}
          fields="legalMonetaryTotal"
          title="Legal Monetary Total"
        />
      </section>

      <section className="invoice-editor-section">
        <h2 className="invoice-editor-section-title">Invoice Lines</h2>

        <form.AppField name="invoiceLine" mode="array">
          {(field) => {
            const invoiceLines = field.state.value ?? [];

            return (
              <fieldset className="form-array-section">

                {invoiceLines.map((_, index) => (
                  <div key={index} className="form-array-item-flat">
                    <InvoiceLine
                      form={form}
                      fields={`invoiceLine[${index}]`}
                      title={`Invoice Line ${index + 1}`}
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
                  </div>
                ))}

                <div className="form-array-actions">
                  <button
                    type="button"
                    className="form-button form-button-primary"
                    onClick={() => {
                      field.pushValue(emptyInvoiceLine);
                    }}
                  >
                    Add
                  </button>
                </div>
              </fieldset>
            );
          }}
        </form.AppField>
      </section>

      <footer className="invoice-editor-footer">
        <button type="submit" className="invoice-submit-button">
          Save
        </button>
      </footer>
    </form>
  );
}