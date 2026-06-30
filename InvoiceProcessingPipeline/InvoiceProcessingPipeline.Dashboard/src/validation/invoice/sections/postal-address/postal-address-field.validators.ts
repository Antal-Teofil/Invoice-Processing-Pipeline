import { required } from "../../shared/common-field.validators";

export const postalAddressFieldValidators = {
  streetName: {},
  additionalStreetName: {},
  cityName: {},
  postalZone: {},
  countrySubentity: {},
  addressLine: {},
  countryCode: {
    onChange: required("Country Code"),
    onSubmit: required("Country Code"),
  },
} as const;
