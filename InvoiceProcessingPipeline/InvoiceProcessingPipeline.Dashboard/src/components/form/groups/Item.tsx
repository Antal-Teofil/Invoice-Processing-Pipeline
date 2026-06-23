import { ItemFormSchema } from "../../../schemas/invoice/item.schema";
import { AdditionalItemPropertyFormSchema } from "../../../schemas/invoice/additional-item-prop.schema";

import { withFieldGroup } from "../setup/invoice-form";
import { TaxCategory } from "./TaxCategory";
import { AdditionalItemProperty } from "./AdditionalItemProperty";
import { COUNTRY_CODE_OPTIONS } from "../../../shared/constants/country-code.constant";

const emptyAdditionalItemProperty =
  AdditionalItemPropertyFormSchema.parse({});

export const Item = withFieldGroup({
  defaultValues: ItemFormSchema.parse({}),

  props: {
    title: "Item",
  },

  render: function Render({ group, title }) {
    return (
      <fieldset>
        <legend>{title}</legend>

        <group.AppField name="name">
          {(field) => <field.TextField label="Item Name" />}
        </group.AppField>

        <group.AppField name="description">
          {(field) => <field.TextField label="Description" />}
        </group.AppField>

        <group.AppField name="countryOriginCode">
          {(field) => (
            <field.OptionField
              label="Country Origin Code"
              options={COUNTRY_CODE_OPTIONS}
            />
          )}
        </group.AppField>

        <TaxCategory
          form={group}
          fields="classifiedTaxCategory"
          title="Classified Tax Category"
        />

        <group.AppField name="additionalItemProperty" mode="array">
          {(field) => {
            const properties = field.state.value ?? [];

            return (
              <fieldset>
                <legend>Additional Item Properties</legend>

                {properties.map((_, index) => (
                  <fieldset key={index}>
                    <AdditionalItemProperty
                      form={group}
                      fields={`additionalItemProperty[${index}]`}
                      title={`Additional Item Property ${index + 1}`}
                    />

                    <button
                      type="button"
                      onClick={() => {
                        field.removeValue(index);
                      }}
                    >
                      Remove
                    </button>
                  </fieldset>
                ))}

                <button
                  type="button"
                  onClick={() => {
                    field.pushValue(emptyAdditionalItemProperty);
                  }}
                >
                  Add
                </button>
              </fieldset>
            );
          }}
        </group.AppField>
      </fieldset>
    );
  },
});