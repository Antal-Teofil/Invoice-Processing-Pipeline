import PartyLegalEntityFormSchema from "../../../schemas/invoice/party-legal-entity.schema";
import { withFieldGroup } from "../setup/invoice-form";

export const PartyLegalEntity = withFieldGroup({
  defaultValues: PartyLegalEntityFormSchema.parse({}),

  props: {
    title: "Party Legal Entity",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="party-legal-entity-section">
        <legend>{title}</legend>

        <group.AppField name="registrationName">
          {(field) => <field.TextField label="Registration Name" />}
        </group.AppField>

        <group.AppField name="companyId">
          {(field) => <field.TextField label="Company ID" />}
        </group.AppField>

        <group.AppField name="companySchemeId">
          {(field) => <field.TextField label="Company Scheme ID" />}
        </group.AppField>

        <group.AppField name="companyLegalForm">
          {(field) => <field.TextField label="Company Legal Form" />}
        </group.AppField>
      </fieldset>
    );
  },
});