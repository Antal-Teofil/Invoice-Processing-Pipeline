import AccountingPartyFormSchema from "../../../schemas/invoice/accounting-party.schema";
import PartyIdentificationFormSchema from "../../../schemas/invoice/party-identification.schema";
import PartyTaxSchemeFormSchema from "../../../schemas/invoice/party-tax-scheme.schema";

import { PartyIdentification } from "./PartyIdentification";

import { withFieldGroup } from "../setup/invoice-form";
import { PostalAddress } from "./Address";
import { Contact } from "./PartyContact";
import { PartyLegalEntity } from "./PartyLegalEntity";
import { PartyTaxScheme } from "./PartyTaxSchema";
import { accountingPartyFieldValidators } from "../../../validation/invoice";
import { FieldErrors } from "../fields/FieldErrors";
import { shouldShowFieldErrors } from "../fields/field-error-visibility";

const emptyPartyIdentification = PartyIdentificationFormSchema.parse({});
const emptyPartyTaxScheme = PartyTaxSchemeFormSchema.parse({});

export const AccountingParty = withFieldGroup({
  defaultValues: AccountingPartyFormSchema.parse({}),

  props: {
    title: "Vendor Party",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="accounting-party-section">
        <legend>{title}</legend>

        <group.AppField name="partyName" validators={accountingPartyFieldValidators.partyName}>
          {(field) => <field.TextField label="Party Name" />}
        </group.AppField>

        <group.AppField name="partyIdentification" mode="array">
          {(field) => {
            const identifications = field.state.value ?? [];
            const showArrayError = shouldShowFieldErrors(field);

            return (
              <fieldset className="form-array-section">
                <legend>Party Identifications</legend>

                <FieldErrors
                  show={showArrayError}
                  errors={field.state.meta.errors}
                  fieldName={field.name}
                />

                {identifications.map((_, index) => (
                  <fieldset key={index} className="form-array-item">
                    <PartyIdentification
                      form={group}
                      fields={`partyIdentification[${index}]`}
                      title={`Party Identification ${index + 1}`}
                    />

                    <div className="form-array-actions">
                      <button
                        type="button"
                        className="form-button form-button-danger"
                        onClick={() => {
                          field.removeValue(index);
                        }}
                      >
                        Remove identification
                      </button>
                    </div>
                  </fieldset>
                ))}

                <div className="form-array-actions">
                  <button
                    type="button"
                    className="form-button form-button-primary"
                    disabled={identifications.length >= 2}
                    onClick={() => {
                      field.pushValue(emptyPartyIdentification);
                    }}
                  >
                    Add identification
                  </button>
                </div>
              </fieldset>
            );
          }}
        </group.AppField>

        <PostalAddress
          form={group}
          fields="postalAddress"
          title="Postal Address"
        />

        <group.AppField name="partyTaxScheme" mode="array">
          {(field) => {
            const taxSchemes = field.state.value ?? [];
            const showArrayError = shouldShowFieldErrors(field);

            return (
              <fieldset className="form-array-section">
                <legend>Party Tax Schemes</legend>

                <FieldErrors
                  show={showArrayError}
                  errors={field.state.meta.errors}
                  fieldName={field.name}
                />

                {taxSchemes.map((_, index) => (
                  <fieldset key={index} className="form-array-item">
                    <PartyTaxScheme
                      form={group}
                      fields={`partyTaxScheme[${index}]`}
                      title={`Party Tax Scheme ${index + 1}`}
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
                      field.pushValue(emptyPartyTaxScheme);
                    }}
                  >
                    Add
                  </button>
                </div>
              </fieldset>
            );
          }}
        </group.AppField>

        <PartyLegalEntity
          form={group}
          fields="partyLegalEntity"
          title="Party Legal Entity"
        />

        <Contact form={group} fields="contact" title="Contact" />
      </fieldset>
    );
  },
});