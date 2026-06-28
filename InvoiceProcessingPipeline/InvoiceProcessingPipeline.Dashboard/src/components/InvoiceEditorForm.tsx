import type { CommercialInvoiceType } from "../types/form/invoice-form.types";
import { useAppForm } from "./form/setup/invoice-form";

import { AccountingParty } from "./form/groups/AccountingParty";
import { InvoiceHeader } from "./form/groups/InvoiceHeader";
import { InvoicePeriodFields } from "./form/groups/InvoicePeriod";
import { LegalMonetaryTotal } from "./form/groups/LegalMonetaryTotal";
import { TaxTotal } from "./form/groups/TaxTotal";
import { AllowanceCharge } from "./form/groups/AllowanceCharge";
import { InvoiceLine } from "./form/groups/InvoiceLine";
import { FormSection } from "./FormSection";

import TaxTotalFormSchema from "../schemas/invoice/tax-total.schema";
import AllowanceChargeFormSchema from "../schemas/invoice/allowance-charge.schema";
import InvoiceLineFormSchema from "../schemas/invoice/invoice-line.schema";

import { useSubmitInvoice } from "../hooks/invoice-form.hook";
import { INVOICE_SECTION_IDS } from "../shared/constants/form-sections.constants";

const emptyTaxTotal = TaxTotalFormSchema.parse({});
const emptyAllowanceCharge = AllowanceChargeFormSchema.parse({});
const emptyInvoiceLine = InvoiceLineFormSchema.parse({});

export function CommercialInvoiceEditorForm({
  documentId,
  invoice,
}: {
  invoice: CommercialInvoiceType;
  documentId: string;
}) {
  const submitInvoice = useSubmitInvoice(documentId);

  const form = useAppForm({
    defaultValues: invoice,

    onSubmit: async ({ value }) => {
      const updatedInvoiceForm = await submitInvoice.mutateAsync(value);

      form.reset(updatedInvoiceForm.data);
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
      <FormSection
        id={INVOICE_SECTION_IDS.invoiceDetails}
        title="Invoice Details"
      >
        <InvoiceHeader form={form} />

        <InvoicePeriodFields
          form={form}
          fields="invoicePeriod"
          title="Invoice Period"
        />
      </FormSection>

      <FormSection id={INVOICE_SECTION_IDS.parties} title="Parties">
        <div id={INVOICE_SECTION_IDS.supplierParty} className="scroll-mt-6">
          <AccountingParty
            form={form}
            fields="accountingSupplierParty"
            title="Supplier Party"
          />
        </div>

        <div id={INVOICE_SECTION_IDS.customerParty} className="scroll-mt-6">
          <AccountingParty
            form={form}
            fields="accountingCustomerParty"
            title="Customer Party"
          />
        </div>
      </FormSection>

      <FormSection
        id={INVOICE_SECTION_IDS.allowanceCharges}
        title="Allowance / Charges"
      >
        <form.AppField name="allowanceCharge" mode="array">
          {(field) => {
            const allowanceCharges = field.state.value ?? [];

            return (
              <fieldset className="form-array-section">
                <legend>Allowance / Charges</legend>

                {allowanceCharges.map((_, index) => (
                  <div
                    key={index}
                    id={INVOICE_SECTION_IDS.allowanceChargeItem(index)}
                    className="form-array-item-flat scroll-mt-6"
                  >
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
      </FormSection>

      <FormSection id={INVOICE_SECTION_IDS.taxTotals} title="Tax Totals">
        <form.AppField name="taxTotal" mode="array">
          {(field) => {
            const taxTotals = field.state.value ?? [];

            return (
              <fieldset className="form-array-section">
                <legend>Tax Totals</legend>

                {taxTotals.map((_, index) => (
                  <div
                    key={index}
                    id={INVOICE_SECTION_IDS.taxTotalItem(index)}
                    className="form-array-item-flat scroll-mt-6"
                  >
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
      </FormSection>

      <FormSection
        id={INVOICE_SECTION_IDS.legalMonetaryTotal}
        title="Legal Monetary Total"
      >
        <LegalMonetaryTotal
          form={form}
          fields="legalMonetaryTotal"
          title="Legal Monetary Total"
        />
      </FormSection>

      <FormSection id={INVOICE_SECTION_IDS.invoiceLines} title="Invoice Lines">
        <form.AppField name="invoiceLine" mode="array">
          {(field) => {
            const invoiceLines = field.state.value ?? [];

            return (
              <fieldset className="form-array-section">
                {invoiceLines.map((_, index) => (
                  <div
                    key={index}
                    id={INVOICE_SECTION_IDS.invoiceLineItem(index)}
                    className="form-array-item-flat scroll-mt-6"
                  >
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
      </FormSection>

      <footer className="invoice-editor-footer">
        <form.AppForm>
          <form.InvoiceSubmitButton isPending={submitInvoice.isPending} />
        </form.AppForm>

        {submitInvoice.isError && (
          <p className="form-error">{submitInvoice.error.message}</p>
        )}
      </footer>
    </form>
  );
}