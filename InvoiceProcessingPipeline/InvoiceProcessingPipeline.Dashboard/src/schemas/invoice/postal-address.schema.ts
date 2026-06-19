import z from "zod";
import { COUNTRY_CODES } from "../../shared/constants/country-code.constant";

const PostalAddressSchema = z.object({
    streetName: z.string().nullable().default(null),
    additionalStreetName: z.string().nullable().default(null),
    cityName: z.string().nullable().default(null),
    postalZone: z.string().nullable().default(null),
    countrySubentity: z.string().nullable().default(null),
    addressLine: z.string().nullable().default(null),
    countryCode: z.enum(COUNTRY_CODES).nullable().default(null),
});

export default PostalAddressSchema;