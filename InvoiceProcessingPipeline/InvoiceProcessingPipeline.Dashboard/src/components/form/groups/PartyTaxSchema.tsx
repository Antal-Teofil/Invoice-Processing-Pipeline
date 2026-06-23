import PartyTaxSchemeFormSchema from "../../../schemas/invoice/party-tax-scheme.schema";
import { withFieldGroup } from "../setup/invoice-form";

export const PartyTaxScheme = withFieldGroup({
  defaultValues: PartyTaxSchemeFormSchema.parse({}),

  props: {
    title: "Party Tax Scheme",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="companyId">
          {(field) => <field.TextField label="Company ID" />}
        </group.AppField>

        <group.AppField name="taxSchemeId">
          {(field) => <field.TextField label="Tax Scheme ID" />}
        </group.AppField>
      </fieldset>
    );
  },
});