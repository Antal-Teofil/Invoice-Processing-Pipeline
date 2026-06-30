import PartyLegalEntityFormSchema from "../../../schemas/invoice/party-legal-entity.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { partyLegalEntityFieldValidators } from "../../../validation/invoice";

export const PartyLegalEntity = withFieldGroup({
  defaultValues: PartyLegalEntityFormSchema.parse({}),

  props: {
    title: "Party Legal Entity",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="party-legal-entity-section">
        <legend>{title}</legend>

        <group.AppField name="registrationName" validators={partyLegalEntityFieldValidators.registrationName}>
          {(field) => <field.TextField label="Registration Name" />}
        </group.AppField>

        <group.AppField name="companyId" validators={partyLegalEntityFieldValidators.companyId}>
          {(field) => <field.TextField label="Company ID" />}
        </group.AppField>

        <group.AppField name="companySchemeId" validators={partyLegalEntityFieldValidators.companySchemeId}>
          {(field) => <field.TextField label="Company Scheme ID" />}
        </group.AppField>

        <group.AppField name="companyLegalForm">
          {(field) => <field.TextField label="Company Legal Form" />}
        </group.AppField>
      </fieldset>
    );
  },
});