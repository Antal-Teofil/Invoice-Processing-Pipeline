import AccountingPartyFormSchema from "../../../schemas/invoice/accounting-party.schema";
import PartyIdentificationFormSchema from "../../../schemas/invoice/party-identification.schema";
import PartyTaxSchemeFormSchema from "../../../schemas/invoice/party-tax-scheme.schema";

import { PartyIdentification } from "../fields/PartyIdentification";

import { withFieldGroup } from "../setup/invoice-form";
import { PostalAddress } from "./Address";
import { Contact } from "./PartyContact";
import { PartyLegalEntity } from "./PartyLegalEntity";
import { PartyTaxScheme } from "./PartyTaxSchema";

const emptyPartyIdentification = PartyIdentificationFormSchema.parse({});
const emptyPartyTaxScheme = PartyTaxSchemeFormSchema.parse({});

export const AccountingParty = withFieldGroup({
  defaultValues: AccountingPartyFormSchema.parse({}),

  props: {
    title: "Vendor Party",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="partyName">
          {(field) => <field.TextField label="Party Name" />}
        </group.AppField>

        <group.AppField name="partyIdentification" mode="array">
          {(field) => {
            const identifications = field.state.value ?? [];

            return (
              <fieldset>
                <legend>Party Identifications</legend>

                {identifications.map((_, index) => (
                  <fieldset key={index}>
                    <PartyIdentification
                      form={group}
                      fields={`partyIdentification[${index}]`}
                      title={`Party Identification ${index + 1}`}
                    />

                    <button
                      type="button"
                      onClick={() => {
                        field.removeValue(index);
                      }}
                    >
                      Remove identification
                    </button>
                  </fieldset>
                ))}

                <button
                  type="button"
                  disabled={identifications.length >= 2}
                  onClick={() => {
                    field.pushValue(emptyPartyIdentification);
                  }}
                >
                  Add identification
                </button>
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

            return (
              <fieldset>
                <legend>Party Tax Schemes</legend>

                {taxSchemes.map((_, index) => (
                  <fieldset key={index}>
                    <PartyTaxScheme
                      form={group}
                      fields={`partyTaxScheme[${index}]`}
                      title={`Party Tax Scheme ${index + 1}`}
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
                    field.pushValue(emptyPartyTaxScheme);
                  }}
                >
                  Add
                </button>
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
