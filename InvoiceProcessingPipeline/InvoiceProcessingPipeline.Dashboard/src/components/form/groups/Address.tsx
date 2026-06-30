import PostalAddressFormSchema from "../../../schemas/invoice/postal-address.schema";
import { COUNTRY_CODE_OPTIONS } from "../../../shared/constants/country-code.constant";
import { withFieldGroup } from "../setup/invoice-form";
import { postalAddressFieldValidators } from "../../../validation/invoice";

export const PostalAddress = withFieldGroup({
  defaultValues: PostalAddressFormSchema.parse({}),

  props: {
    title: "Postal Address",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset className="postal-address-section">
        <legend>{title}</legend>

        <group.AppField name="streetName" validators={postalAddressFieldValidators.streetName}>
          {(field) => <field.TextField label="Street Name" />}
        </group.AppField>

        <group.AppField name="additionalStreetName">
          {(field) => <field.TextField label="Additional Street Name" />}
        </group.AppField>

        <group.AppField name="cityName" validators={postalAddressFieldValidators.cityName}>
          {(field) => <field.TextField label="City" />}
        </group.AppField>

        <group.AppField name="postalZone" validators={postalAddressFieldValidators.postalZone}>
          {(field) => <field.TextField label="Postal Zone" />}
        </group.AppField>

        <group.AppField name="countrySubentity">
          {(field) => <field.TextField label="Country Subentity" />}
        </group.AppField>

        <group.AppField name="addressLine" validators={postalAddressFieldValidators.addressLine}>
          {(field) => <field.TextField label="Address Line" />}
        </group.AppField>

        <group.AppField name="countryCode" validators={postalAddressFieldValidators.countryCode}>
          {(field) => (
            <field.OptionField
              label="Country Code"
              options={COUNTRY_CODE_OPTIONS}
            />
          )}
        </group.AppField>
      </fieldset>
    );
  },
});