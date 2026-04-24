import type { ReactNode } from "react";
import { useForm } from "@tanstack/react-form";

import { useInvoiceData } from "../hooks/use-invoice";

import {
  COUNTRY_CODES,
  CURRENCY_CODES,
  INVOICE_TYPE_CODES,
} from "../types/invoice-type-codes";

import {
  createDefaultInvoiceLine,
  type Invoice,
  type InvoiceLine,
} from "../scheme/InvoiceScheme";

type CodeLabelOption = {
  readonly code: string;
  readonly label: string;
};

export function InvoiceEditorForm({ invoiceId }: { invoiceId: string }) {
  const { data, isLoading, error } = useInvoiceData(invoiceId);

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-slate-50 text-slate-600">
        Loading form...
      </div>
    );
  }

  if (error) {
    return (
      <div className="mx-auto mt-8 max-w-3xl rounded-2xl border border-red-200 bg-red-50 p-6 text-red-700">
        Error: {error.message}
      </div>
    );
  }

  if (!data) {
    return (
      <div className="mx-auto mt-8 max-w-3xl rounded-2xl border border-slate-200 bg-white p-6 text-slate-600">
        No invoice found
      </div>
    );
  }

  return <InvoiceEditorFormContent data={data} />;
}

export function InvoiceEditorFormContent({ data }: { data: Invoice }) {
  const form = useForm({
    defaultValues: {
      data,
    },

    onSubmit: async ({ value }) => {
      console.log("TANSTACK FORM SUBMIT");
      console.log("Submitted invoice:", value.data);
    },
  });

  const FieldError = ({ errors }: { errors: unknown[] }) => {
    if (errors.length === 0) return null;

    return (
      <p className="mt-1 text-sm text-red-600">
        {errors.map(String).join(", ")}
      </p>
    );
  };

  const Section = ({
    title,
    description,
    children,
  }: {
    title: string;
    description?: string;
    children: ReactNode;
  }) => (
    <section className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
      <div className="mb-5 border-b border-slate-100 pb-4">
        <h2 className="text-lg font-semibold text-slate-900">{title}</h2>

        {description && (
          <p className="mt-1 text-sm text-slate-500">{description}</p>
        )}
      </div>

      {children}
    </section>
  );

  const TextField = ({
    name,
    label,
    type = "text",
  }: {
    name: string;
    label: string;
    type?: string;
  }) => (
    <form.Field
      name={name as any}
      children={(field) => {
        const handleChange = field.handleChange as unknown as (
          value: string
        ) => void;

        return (
          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700">
              {label}
            </label>

            <input
              type={type}
              value={(field.state.value ?? "") as string}
              onChange={(e) => handleChange(e.target.value)}
              onBlur={field.handleBlur}
              className="w-full rounded-xl border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 shadow-sm outline-none transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100"
            />

            <FieldError errors={field.state.meta.errors} />
          </div>
        );
      }}
    />
  );

  const NumberField = ({ name, label }: { name: string; label: string }) => (
    <form.Field
      name={name as any}
      children={(field) => {
        const handleChange = field.handleChange as unknown as (
          value: number
        ) => void;

        return (
          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700">
              {label}
            </label>

            <input
              type="number"
              value={(field.state.value ?? 0) as number}
              onChange={(e) =>
                handleChange(
                  e.target.value === "" ? 0 : e.target.valueAsNumber
                )
              }
              onBlur={field.handleBlur}
              className="w-full rounded-xl border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 shadow-sm outline-none transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100"
            />

            <FieldError errors={field.state.meta.errors} />
          </div>
        );
      }}
    />
  );

  const SelectField = ({
    name,
    label,
    options,
  }: {
    name: string;
    label: string;
    options: readonly CodeLabelOption[];
  }) => (
    <form.Field
      name={name as any}
      children={(field) => {
        const handleChange = field.handleChange as unknown as (
          value: string
        ) => void;

        return (
          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700">
              {label}
            </label>

            <select
              value={(field.state.value ?? "") as string}
              onChange={(e) => handleChange(e.target.value)}
              onBlur={field.handleBlur}
              className="w-full rounded-xl border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 shadow-sm outline-none transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100"
            >
              {options.map((option) => (
                <option key={option.code} value={option.code}>
                  {`${option.code} - ${option.label}`}
                </option>
              ))}
            </select>

            <FieldError errors={field.state.meta.errors} />
          </div>
        );
      }}
    />
  );

  const PartySection = ({
    title,
    prefix,
  }: {
    title: string;
    prefix: "accountingCustomerParty" | "accountingSupplierParty";
  }) => (
    <Section title={title}>
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
        <TextField name={`data.${prefix}.businessName`} label="Business Name" />
        <TextField name={`data.${prefix}.address.streetName`} label="Street Name" />
        <TextField name={`data.${prefix}.address.additionalStreetName`} label="Additional Street Name" />
        <TextField name={`data.${prefix}.address.cityName`} label="City Name" />
        <TextField name={`data.${prefix}.address.postalZone`} label="Postal Zone" />
        <TextField name={`data.${prefix}.address.countrySubEntity`} label="Country Subentity" />
        <TextField name={`data.${prefix}.address.addressLine`} label="Address Line" />

        <SelectField
          name={`data.${prefix}.address.countryCode`}
          label="Country Code"
          options={COUNTRY_CODES}
        />

        <TextField name={`data.${prefix}.contact.contactName`} label="Contact Name" />
        <TextField name={`data.${prefix}.contact.telephoneNumber`} label="Telephone Number" />
        <TextField name={`data.${prefix}.contact.electronicMail`} label="Electronic Mail" />
        <TextField name={`data.${prefix}.partyEntity.registrationName`} label="Registration Name" />
        <TextField name={`data.${prefix}.partyEntity.companyId`} label="Company Identifier" />
        <TextField name={`data.${prefix}.partyTaxScheme.companyId`} label="Tax Identifier" />
        <TextField name={`data.${prefix}.partyTaxScheme.taxScheme`} label="Tax Scheme" />
      </div>
    </Section>
  );

  return (
    <div className="min-h-screen bg-slate-50 px-4 py-8">
      <div className="mx-auto max-w-7xl">
        <div className="mb-8">
          <p className="text-sm font-medium uppercase tracking-wide text-indigo-600">
            Invoice editor
          </p>

          <h1 className="mt-2 text-3xl font-bold tracking-tight text-slate-950">
            INVOICE
          </h1>

          <p className="mt-2 text-sm text-slate-500">
            Edit invoice header, parties, dates, currencies and line items.
          </p>
        </div>

        <form
          className="space-y-6"
          onSubmit={(e) => {
            e.preventDefault();
            e.stopPropagation();
            void form.handleSubmit();
          }}
        >
          <Section
            title="Invoice information"
            description="Basic invoice identifiers, dates, type and currencies."
          >
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3">
              <TextField name="data.invoiceId" label="Invoice Identifier" />

              <SelectField
                name="data.typeCode"
                label="Invoice Type Code"
                options={INVOICE_TYPE_CODES}
              />

              <TextField name="data.note" label="Invoice Note" />
              <TextField name="data.issueDate" label="Issue Date" type="date" />
              <TextField name="data.dueDate" label="Due Date" type="date" />
              <TextField name="data.taxPointDate" label="Tax Point Date" type="date" />

              <SelectField
                name="data.documentCurrencyCode"
                label="Document Currency Code"
                options={CURRENCY_CODES}
              />

              <SelectField
                name="data.taxCurrencyCode"
                label="Tax Currency Code"
                options={CURRENCY_CODES}
              />
            </div>
          </Section>

          <div className="grid grid-cols-1 gap-6 xl:grid-cols-2">
            <PartySection
              title="Customer party"
              prefix="accountingCustomerParty"
            />

            <PartySection
              title="Supplier party"
              prefix="accountingSupplierParty"
            />
          </div>

          <Section
            title="Invoice lines"
            description="Manage line items, products, quantities, VAT and amounts."
          >
            <form.Field
              name={"data.lineItems" as any}
              mode="array"
              children={(lineItemsField) => {
                const lineItems = lineItemsField.state.value as InvoiceLine[];

                const pushInvoiceLine = lineItemsField.pushValue as unknown as (
                  value: InvoiceLine
                ) => void;

                const removeInvoiceLine =
                  lineItemsField.removeValue as unknown as (
                    index: number
                  ) => void;

                return (
                  <div className="space-y-5">
                    {lineItems.map((_, index) => (
                      <div
                        key={index}
                        className="rounded-2xl border border-slate-200 bg-slate-50 p-5"
                      >
                        <div className="mb-5 flex items-center justify-between gap-4 border-b border-slate-200 pb-4">
                          <div>
                            <h3 className="text-base font-semibold text-slate-900">
                              Line {index + 1}
                            </h3>

                            <p className="text-sm text-slate-500">
                              Invoice line details and item classification.
                            </p>
                          </div>

                          <button
                            type="button"
                            onClick={() => removeInvoiceLine(index)}
                            className="rounded-xl border border-red-200 bg-white px-3 py-2 text-sm font-medium text-red-600 shadow-sm transition hover:bg-red-50"
                          >
                            Remove line
                          </button>
                        </div>

                        <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3">
                          <TextField name={`data.lineItems[${index}].lineId`} label="Line ID" />
                          <TextField name={`data.lineItems[${index}].note`} label="Note" />

                          <NumberField
                            name={`data.lineItems[${index}].invoicedQuantity`}
                            label="Invoiced Quantity"
                          />

                          <TextField
                            name={`data.lineItems[${index}].invoicedQuantityUnitCode`}
                            label="Quantity Unit Code"
                          />

                          <NumberField
                            name={`data.lineItems[${index}].priceAmount`}
                            label="Price Amount"
                          />

                          <SelectField
                            name={`data.lineItems[${index}].priceAmountCurrencyId`}
                            label="Price Currency"
                            options={CURRENCY_CODES}
                          />

                          <NumberField
                            name={`data.lineItems[${index}].baseQuantity`}
                            label="Base Quantity"
                          />

                          <TextField
                            name={`data.lineItems[${index}].baseQuantityUnitCode`}
                            label="Base Quantity Unit Code"
                          />

                          <NumberField
                            name={`data.lineItems[${index}].lineExtensionAmount`}
                            label="Line Net Amount"
                          />

                          <SelectField
                            name={`data.lineItems[${index}].lineExtensionAmountCurrencyId`}
                            label="Line Currency"
                            options={CURRENCY_CODES}
                          />
                        </div>

                        <div className="mt-6 grid grid-cols-1 gap-6 xl:grid-cols-2">
                          <div className="rounded-2xl border border-slate-200 bg-white p-5">
                            <h4 className="mb-4 text-sm font-semibold uppercase tracking-wide text-slate-600">
                              Item
                            </h4>

                            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                              <TextField
                                name={`data.lineItems[${index}].item.name`}
                                label="Item Name"
                              />

                              <TextField
                                name={`data.lineItems[${index}].item.description`}
                                label="Item Description"
                              />

                              <TextField
                                name={`data.lineItems[${index}].item.sellersItemIdentification`}
                                label="Seller Item Identification"
                              />

                              <TextField
                                name={`data.lineItems[${index}].item.originCountry`}
                                label="Origin Country"
                              />

                              <SelectField
                                name={`data.lineItems[${index}].item.countryCode`}
                                label="Country Code"
                                options={COUNTRY_CODES}
                              />
                            </div>
                          </div>

                          <div className="rounded-2xl border border-slate-200 bg-white p-5">
                            <h4 className="mb-4 text-sm font-semibold uppercase tracking-wide text-slate-600">
                              VAT classification
                            </h4>

                            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                              <TextField
                                name={`data.lineItems[${index}].item.classifiedTaxCategory.vatCategoryCodeId`}
                                label="VAT Category"
                              />

                              <NumberField
                                name={`data.lineItems[${index}].item.classifiedTaxCategory.percent`}
                                label="VAT Percent"
                              />

                              <TextField
                                name={`data.lineItems[${index}].item.classifiedTaxCategory.taxScheme`}
                                label="Tax Scheme"
                              />
                            </div>
                          </div>
                        </div>

                        <div className="mt-6 rounded-2xl border border-slate-200 bg-white p-5">
                          <h4 className="mb-4 text-sm font-semibold uppercase tracking-wide text-slate-600">
                            Allowance / charge
                          </h4>

                          <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3">
                            <NumberField
                              name={`data.lineItems[${index}].allowanceCharge.amount`}
                              label="Amount"
                            />

                            <SelectField
                              name={`data.lineItems[${index}].allowanceCharge.amountCurrencyId`}
                              label="Amount Currency"
                              options={CURRENCY_CODES}
                            />

                            <NumberField
                              name={`data.lineItems[${index}].allowanceCharge.baseAmount`}
                              label="Base Amount"
                            />

                            <SelectField
                              name={`data.lineItems[${index}].allowanceCharge.baseAmountCurrencyId`}
                              label="Base Amount Currency"
                              options={CURRENCY_CODES}
                            />

                            <NumberField
                              name={`data.lineItems[${index}].allowanceCharge.multiplierFactorNumeric`}
                              label="Multiplier Factor"
                            />

                            <TextField
                              name={`data.lineItems[${index}].allowanceCharge.allowanceChargeReason`}
                              label="Reason"
                            />
                          </div>
                        </div>
                      </div>
                    ))}

                    <button
                      type="button"
                      onClick={() => pushInvoiceLine(createDefaultInvoiceLine())}
                      className="rounded-xl bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm transition hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-200"
                    >
                      Add invoice line
                    </button>
                  </div>
                );
              }}
            />
          </Section>

          <div className="sticky bottom-0 rounded-2xl border border-slate-200 bg-white/90 p-4 shadow-lg backdrop-blur">
            <div className="flex items-center justify-end gap-3">
              <button
                type="button"
                onClick={() => form.reset()}
                className="rounded-xl border border-slate-300 bg-white px-4 py-2 text-sm font-semibold text-slate-700 shadow-sm transition hover:bg-slate-50"
              >
                Reset
              </button>

              <form.Subscribe
                selector={(state) => [state.canSubmit, state.isSubmitting]}
                children={([canSubmit, isSubmitting]) => (
                  <button
                    type="submit"
                    disabled={!canSubmit}
                    className="rounded-xl bg-slate-950 px-5 py-2 text-sm font-semibold text-white shadow-sm transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-50"
                  >
                    {isSubmitting ? "Saving..." : "Save invoice"}
                  </button>
                )}
              />
            </div>
          </div>
        </form>
      </div>
    </div>
  );
}