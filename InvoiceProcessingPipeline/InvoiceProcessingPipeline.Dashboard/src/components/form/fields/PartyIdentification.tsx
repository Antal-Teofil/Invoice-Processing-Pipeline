import PartyIdentificationFormSchema from "../../../schemas/invoice/party-identification.schema";
import { withFieldGroup } from "../setup/invoice-form";

export const PartyIdentification = withFieldGroup({
  defaultValues: PartyIdentificationFormSchema.parse({}),

  props: {
    title: "Party Identification",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="id">
          {(field) => <field.TextField label="ID" />}
        </group.AppField>

        <group.AppField name="schemeId">
          {(field) => <field.TextField label="Scheme ID" />}
        </group.AppField>
      </fieldset>
    );
  },
});