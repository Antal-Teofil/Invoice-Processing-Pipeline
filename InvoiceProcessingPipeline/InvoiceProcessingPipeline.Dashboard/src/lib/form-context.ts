import { createFormHook, createFormHookContexts } from '@tanstack/react-form'
import TextField from '../components/form/fields/TextField';
import { NumberField } from '../components/form/fields/NumberField';
import DateField from '../components/form/fields/DateField';
import OptionField from '../components/form/fields/OptionField';
import { EmailField } from '../components/form/fields/Email';
import { CheckboxField } from '../components/form/fields/CheckBox';

export const { fieldContext, useFieldContext, formContext, useFormContext} = createFormHookContexts();

export const { useAppForm } = createFormHook({
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
