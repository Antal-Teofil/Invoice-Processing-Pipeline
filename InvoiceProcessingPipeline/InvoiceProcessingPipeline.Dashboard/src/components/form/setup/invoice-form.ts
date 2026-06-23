import { createFormHook } from "@tanstack/react-form";
import { fieldContext, formContext } from "../../../lib/form-context";
import TextField from "../fields/TextField";
import { NumberField } from "../fields/NumberField";
import DateField from "../fields/DateField";
import OptionField from "../fields/OptionField";
import { EmailField } from "../fields/Email";
import { CheckboxField } from "../fields/CheckBox";

export const {
  useAppForm,
  withForm,
  withFieldGroup,
} = createFormHook({
  fieldContext,
  formContext,
  fieldComponents: {
    TextField,
    NumberField,
    DateField,
    OptionField,
    EmailField,
    CheckboxField
  },
  formComponents: {},
});