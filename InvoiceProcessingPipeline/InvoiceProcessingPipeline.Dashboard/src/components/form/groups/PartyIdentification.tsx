import PartyIdentificationFormSchema from "../../../schemas/invoice/party-identification.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { partyIdentificationFieldValidators } from "../../../validation/invoice";

export const PartyIdentification = withFieldGroup({
  defaultValues: PartyIdentificationFormSchema.parse({}),

  props: {
    title: "Party Identification",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="form-group">
        <legend>{title}</legend>

        <group.AppField name="id" validators={partyIdentificationFieldValidators.id}>
          {(field) => <field.TextField label="ID" />}
        </group.AppField>

        <group.AppField name="schemeId" validators={partyIdentificationFieldValidators.schemeId}>
          {(field) => <field.TextField label="Scheme ID" />}
        </group.AppField>
      </fieldset>
    );
  },
});