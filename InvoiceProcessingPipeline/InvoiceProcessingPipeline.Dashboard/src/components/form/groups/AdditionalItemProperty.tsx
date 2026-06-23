import { AdditionalItemPropertyFormSchema } from "../../../schemas/invoice/additional-item-prop.schema";
import { withFieldGroup } from "../setup/invoice-form";

export const AdditionalItemProperty = withFieldGroup({
  defaultValues: AdditionalItemPropertyFormSchema.parse({}),

  props: {
    title: "Additional Item Property",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="name">
          {(field) => <field.TextField label="Name" />}
        </group.AppField>

        <group.AppField name="propertyValue">
          {(field) => <field.TextField label="Property Value" />}
        </group.AppField>
      </fieldset>
    );
  },
});