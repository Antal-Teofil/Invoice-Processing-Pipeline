import { useFormContext } from "../../../lib/form-context";


type InvoiceSubmitButtonProps = {
  isPending?: boolean;
};

export function InvoiceSubmitButton({
  isPending = false,
}: InvoiceSubmitButtonProps) {
  const form = useFormContext();

  return (
    <form.Subscribe
      selector={(state) => [state.canSubmit, state.isSubmitting] as const}
    >
      {([canSubmit, isSubmitting]) => (
        <button
          type="submit"
          className="invoice-submit-button"
          disabled={!canSubmit || isSubmitting || isPending}
        >
          {isSubmitting || isPending ? 'Saving...' : 'Save'}
        </button>
      )}
    </form.Subscribe>
  );
}