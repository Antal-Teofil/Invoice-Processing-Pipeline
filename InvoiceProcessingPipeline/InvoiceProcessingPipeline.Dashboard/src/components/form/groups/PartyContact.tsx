import ContactFormSchema from "../../../schemas/invoice/contact.schema";
import { withFieldGroup } from "../setup/invoice-form";
import { contactFieldValidators } from "../../../validation/invoice";

export const Contact = withFieldGroup({
  defaultValues: ContactFormSchema.parse({}),

  props: {
    title: "Contact",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="contact-section">
        <legend>{title}</legend>

        <group.AppField name="name">
          {(field) => <field.TextField label="Name" />}
        </group.AppField>

        <group.AppField name="telephone" validators={contactFieldValidators.telephone}>
          {(field) => <field.TextField label="Telephone" />}
        </group.AppField>

        <group.AppField name="electronicMail" validators={contactFieldValidators.electronicMail}>
          {(field) => <field.EmailField label="Email" />}
        </group.AppField>
      </fieldset>
    );
  },
});