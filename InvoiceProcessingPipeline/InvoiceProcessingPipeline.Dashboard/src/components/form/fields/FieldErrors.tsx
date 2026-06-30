import {
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
  type CSSProperties,
} from "react";
import { createPortal } from "react-dom";

function normalizeFieldId(fieldName: string) {
  return fieldName.replace(/[^a-zA-Z0-9_-]+/g, "-");
}

export function getFieldErrorDialogId(fieldName: string) {
  return `${normalizeFieldId(fieldName)}-validation-dialog`;
}

type FieldErrorsProps = {
  show: boolean;
  errors: unknown[];
  fieldName: string;
};

type DialogPlacement = "top" | "bottom";

type DialogPosition = {
  top: number;
  left: number;
  width: number;
  arrowLeft: number;
  placement: DialogPlacement;
};

type ValidationInsight = {
  message: string;
  businessContext: string;
  recommendedFix: string;
};

const VIEWPORT_PADDING = 16;
const DIALOG_GAP = 12;
const DIALOG_MAX_WIDTH = 430;
const DIALOG_MIN_WIDTH = 320;

function clamp(value: number, min: number, max: number) {
  return Math.min(Math.max(value, min), max);
}

function toFriendlyFieldName(fieldName: string) {
  const lastSegment =
    fieldName
      .replace(/\[(\d+)\]/g, " #$1")
      .split(".")
      .filter(Boolean)
      .at(-1) ?? fieldName;

  return lastSegment
    .replace(/([a-z])([A-Z])/g, "$1 $2")
    .replace(/[_-]+/g, " ")
    .replace(/\b\w/g, (letter) => letter.toUpperCase());
}

function getBusinessContextForError(error: string, fieldName: string) {
  const lowerError = error.toLowerCase();
  const lowerFieldName = fieldName.toLowerCase();

  if (lowerFieldName.includes("issuedate")) {
    return "The issue date anchors the accounting period, tax reporting period and several downstream date checks. Without it, the invoice cannot be reliably posted or matched to a reporting period.";
  }

  if (lowerFieldName.includes("duedate")) {
    return "The due date drives payment scheduling, overdue monitoring and cash-flow forecasting. It must stay consistent with the issue date and the payment terms used by the document.";
  }

  if (lowerFieldName.includes("taxpointdate")) {
    return "The tax point date determines when the tax liability is recognized. Incorrect chronology can move the transaction into the wrong VAT reporting period.";
  }

  if (lowerFieldName.includes("documentcurrencycode")) {
    return "The document currency controls how all invoice totals are interpreted, validated and exported. Missing or inconsistent currency data can cause incorrect ledger amounts or payment mismatches.";
  }

  if (lowerFieldName.includes("taxcurrencycode")) {
    return "The tax currency is used when tax amounts need to be reported in a currency different from the invoice currency. It should only differ when the business scenario requires it.";
  }

  if (lowerFieldName.includes("typecode")) {
    return "The invoice type defines the business meaning of the document, such as invoice, credit note or correction. It affects which fields are mandatory and how the document is processed.";
  }

  if (lowerFieldName.includes("partyname")) {
    return "The party name identifies the trading partner shown on the invoice. It is used for audit trails, supplier/customer matching and accounting records.";
  }

  if (
    lowerFieldName.includes("postaladdress") ||
    lowerFieldName.includes("countrycode")
  ) {
    return "Address and country data identify the tax jurisdiction and support supplier/customer validation. Missing country information can block tax treatment, reporting and cross-border checks.";
  }

  if (
    lowerFieldName.includes("partyidentification") ||
    lowerError.includes("identifier")
  ) {
    return "Party identifiers connect the invoice to the correct legal or tax entity. They are important for duplicate prevention, partner matching and auditability.";
  }

  if (
    lowerFieldName.includes("schemeid") ||
    lowerError.includes("scheme id") ||
    lowerError.includes("scheme code")
  ) {
    return "Identifier scheme codes explain what kind of business identifier is being used. They help receiving systems interpret company, registration and tax identifiers correctly.";
  }

  if (lowerFieldName.includes("taxcategory")) {
    return "The tax category describes the VAT treatment of the line or tax subtotal. It directly affects tax calculation, exemption handling and reverse-charge/export scenarios.";
  }

  if (lowerFieldName.includes("taxscheme")) {
    return "The tax scheme clarifies which tax system the identifier or tax category belongs to. VAT-related data must be marked consistently so tax totals can be validated.";
  }

  if (lowerFieldName.includes("taxpercent") || lowerError.includes("percent")) {
    return "The tax percentage is used to calculate the tax amount from the taxable base. A wrong rate can produce incorrect tax totals and payment amounts.";
  }

  if (lowerFieldName.includes("lineextensionamount")) {
    return "Line extension amounts are the net line values used to build the invoice totals. Any mismatch here will propagate into legal monetary totals and accounting export.";
  }

  if (lowerFieldName.includes("payableamount")) {
    return "The payable amount is the final amount expected to be paid. It must reconcile with line totals, charges, allowances, taxes, prepayments and rounding.";
  }

  if (
    lowerFieldName.includes("allowance") ||
    lowerFieldName.includes("charge")
  ) {
    return "Allowances and charges change the invoice net and payable totals. They must be calculated consistently so discounts, fees and surcharges are reflected correctly.";
  }

  if (
    lowerFieldName.includes("amount") ||
    lowerError.includes("expected") ||
    lowerError.includes("must equal") ||
    lowerError.includes("does not match")
  ) {
    return "This amount participates in invoice reconciliation. If it does not match the related source values, the invoice total cannot be trusted for posting, payment or export.";
  }

  if (
    lowerFieldName.includes("unitcode") ||
    lowerError.includes("unit code") ||
    lowerError.includes("unit of measure")
  ) {
    return "The unit code defines how the quantity is measured. It is required for consistent line calculations, receiving-system interpretation and inventory or service reporting.";
  }

  if (lowerFieldName.includes("quantity")) {
    return "Quantity is part of the line calculation together with price and base quantity. Incorrect quantities can change line totals, tax bases and payable amounts.";
  }

  if (lowerFieldName.includes("email")) {
    return "A valid email address helps the receiving workflow contact the correct party and supports automated notifications or exception handling.";
  }

  if (
    lowerFieldName.includes("telephone") ||
    lowerError.includes("phone") ||
    lowerError.includes("telephone")
  ) {
    return "A usable telephone number helps operational teams resolve invoice exceptions with the correct contact.";
  }

  if (
    lowerError.includes("date") ||
    lowerError.includes("earlier") ||
    lowerError.includes("later")
  ) {
    return "Date consistency is important for tax period selection, payment terms and audit chronology. Related dates should describe a coherent business timeline.";
  }

  if (lowerError.includes("currency")) {
    return "Currency consistency is necessary for accurate totals, exchange-rate handling and payment matching across the invoice.";
  }

  if (lowerError.includes("required")) {
    return "This value is required for the structured invoice to be complete. Leaving it empty can prevent validation, automated posting or downstream processing.";
  }

  return "This field is part of the structured invoice data used for validation, tax checks, accounting export and payment processing.";
}

function getSuggestionForError(error: string, fieldName: string) {
  const lowerError = error.toLowerCase();
  const lowerFieldName = fieldName.toLowerCase();

  if (lowerFieldName.includes("documentcurrencycode")) {
    return "Select the currency printed on the invoice header. Then check that all amount currency fields use the same currency unless a separate tax currency is intentionally required.";
  }

  if (lowerFieldName.includes("countrycode")) {
    return "Select the country where the party is legally established or where the postal address belongs. Use the same country as the party identifiers and VAT number when applicable.";
  }

  if (lowerFieldName.includes("partyname")) {
    return "Enter the legal trading name exactly as it appears on the invoice or in the master data for this supplier/customer.";
  }

  if (lowerFieldName.includes("typecode")) {
    return "Choose the document type that matches the invoice content. Use a normal commercial invoice type for standard invoices and a correction/credit type only when the document reverses or adjusts a previous invoice.";
  }

  if (lowerFieldName.includes("unitcode")) {
    return "Choose the closest supported unit from the dropdown, for example Piece, Kilogram, Litre, Metre or Hour. Avoid free-text units because receiving systems cannot reliably interpret them.";
  }

  if (lowerFieldName.includes("taxcategory")) {
    return "Select the VAT category that matches the business scenario, such as standard rated, zero rated, exempt, reverse charge, intra-community supply, export or out of scope. Then verify the tax rate and tax scheme.";
  }

  if (lowerFieldName.includes("taxscheme")) {
    return "Use VAT for VAT-related tax identifiers and categories. If the transaction is not VAT-based, review the selected tax category and remove the inconsistent tax identifier.";
  }

  if (
    lowerFieldName.includes("schemeid") ||
    lowerError.includes("scheme id") ||
    lowerError.includes("scheme code")
  ) {
    return "Provide a four-digit identifier scheme code that describes the related identifier. If the identifier is not available, remove both the identifier and its scheme code instead of leaving only one side filled.";
  }

  if (
    lowerError.includes("expected") ||
    lowerError.includes("must equal") ||
    lowerError.includes("does not match")
  ) {
    return "Compare the expected value shown in the error with the invoice PDF and the related source fields. Correct either this total or the underlying line, charge, allowance, tax, prepaid or rounding values that produce it.";
  }

  if (lowerError.includes("required")) {
    return "Fill in this value before saving. If the value should be derived from another section, complete the source section first and then return to this field if it still needs manual confirmation.";
  }

  if (
    lowerError.includes("cannot be earlier") ||
    lowerError.includes("cannot be later") ||
    lowerError.includes("date")
  ) {
    return "Review the related dates and adjust them so the invoice timeline is logical, for example period start before period end and due date not before issue date.";
  }

  if (lowerError.includes("currency")) {
    return "Use the document currency consistently across totals and amount fields. Only set a different tax currency when the invoice explicitly requires tax reporting in another currency.";
  }

  if (lowerError.includes("identifier")) {
    return "Add the missing identifier in the related party section. Prefer the identifier shown on the invoice or already stored in partner master data.";
  }

  if (lowerError.includes("email")) {
    return "Enter a complete email address with a local part, @ sign and domain, for example billing@example.com.";
  }

  if (lowerError.includes("phone") || lowerError.includes("telephone")) {
    return "Enter a reachable phone number using digits and, when available, the international prefix.";
  }

  return "Review this value together with the related invoice section and update the field so the invoice can be validated and saved.";
}

function buildValidationInsight(
  error: string,
  fieldName: string,
): ValidationInsight {
  return {
    message: error,
    businessContext: getBusinessContextForError(error, fieldName),
    recommendedFix: getSuggestionForError(error, fieldName),
  };
}

export function FieldErrors({ show, errors, fieldName }: FieldErrorsProps) {
  const anchorRef = useRef<HTMLSpanElement>(null);
  const dialogRef = useRef<HTMLDivElement>(null);
  const closeTimerRef = useRef<number | null>(null);
  const [isOpen, setIsOpen] = useState(false);
  const [position, setPosition] = useState<DialogPosition | null>(null);

  const visibleErrors = useMemo(() => {
    if (!show || errors.length === 0) {
      return [];
    }

    return Array.from(new Set(errors.map(String).filter(Boolean)));
  }, [errors, show]);

  const insights = useMemo(() => {
    return visibleErrors.map((error) =>
      buildValidationInsight(error, fieldName),
    );
  }, [fieldName, visibleErrors]);

  const clearCloseTimer = useCallback(() => {
    if (closeTimerRef.current !== null) {
      window.clearTimeout(closeTimerRef.current);
      closeTimerRef.current = null;
    }
  }, []);

  const updatePosition = useCallback(() => {
    const fieldElement = anchorRef.current?.parentElement;

    if (!fieldElement || typeof window === "undefined") {
      return;
    }

    const fieldRect = fieldElement.getBoundingClientRect();
    const viewportWidth = window.innerWidth;
    const viewportHeight = window.innerHeight;
    const width = Math.min(
      DIALOG_MAX_WIDTH,
      Math.max(DIALOG_MIN_WIDTH, viewportWidth - VIEWPORT_PADDING * 2),
    );

    const measuredHeight = dialogRef.current?.offsetHeight ?? 360;
    const hasRoomBelow =
      fieldRect.bottom + DIALOG_GAP + measuredHeight <=
      viewportHeight - VIEWPORT_PADDING;
    const hasRoomAbove =
      fieldRect.top - DIALOG_GAP - measuredHeight >= VIEWPORT_PADDING;
    const placement: DialogPlacement =
      !hasRoomBelow && hasRoomAbove ? "top" : "bottom";

    const left = clamp(
      fieldRect.left,
      VIEWPORT_PADDING,
      Math.max(VIEWPORT_PADDING, viewportWidth - width - VIEWPORT_PADDING),
    );

    const rawTop =
      placement === "top"
        ? fieldRect.top - DIALOG_GAP - measuredHeight
        : fieldRect.bottom + DIALOG_GAP;

    const top = clamp(
      rawTop,
      VIEWPORT_PADDING,
      Math.max(
        VIEWPORT_PADDING,
        viewportHeight - measuredHeight - VIEWPORT_PADDING,
      ),
    );

    const fieldCenter = fieldRect.left + fieldRect.width / 2;
    const arrowLeft = clamp(fieldCenter - left, 28, width - 28);

    setPosition({ top, left, width, arrowLeft, placement });
  }, []);

  const openDialog = useCallback(() => {
    if (visibleErrors.length === 0) {
      return;
    }

    clearCloseTimer();
    setIsOpen(true);
    window.requestAnimationFrame(updatePosition);
  }, [clearCloseTimer, updatePosition, visibleErrors.length]);

  const closeDialogSoon = useCallback(() => {
    clearCloseTimer();
    closeTimerRef.current = window.setTimeout(() => {
      setIsOpen(false);
    }, 120);
  }, [clearCloseTimer]);

  useEffect(() => {
    if (visibleErrors.length === 0) {
      setIsOpen(false);
      setPosition(null);
    }
  }, [visibleErrors.length]);

  useEffect(() => {
    const fieldElement = anchorRef.current?.parentElement;

    if (!fieldElement || visibleErrors.length === 0) {
      return undefined;
    }

    const handleFocusOut = () => {
      window.setTimeout(() => {
        if (!fieldElement.contains(document.activeElement)) {
          closeDialogSoon();
        }
      }, 0);
    };

    fieldElement.addEventListener("mouseenter", openDialog);
    fieldElement.addEventListener("mouseleave", closeDialogSoon);
    fieldElement.addEventListener("focusin", openDialog);
    fieldElement.addEventListener("focusout", handleFocusOut);

    return () => {
      fieldElement.removeEventListener("mouseenter", openDialog);
      fieldElement.removeEventListener("mouseleave", closeDialogSoon);
      fieldElement.removeEventListener("focusin", openDialog);
      fieldElement.removeEventListener("focusout", handleFocusOut);
    };
  }, [closeDialogSoon, openDialog, visibleErrors.length]);

  useEffect(() => {
    if (!isOpen) {
      return undefined;
    }

    updatePosition();
    const animationFrame = window.requestAnimationFrame(updatePosition);

    window.addEventListener("resize", updatePosition);
    window.addEventListener("scroll", updatePosition, true);

    return () => {
      window.cancelAnimationFrame(animationFrame);
      window.removeEventListener("resize", updatePosition);
      window.removeEventListener("scroll", updatePosition, true);
    };
  }, [insights, isOpen, updatePosition]);

  useEffect(() => {
    return () => {
      clearCloseTimer();
    };
  }, [clearCloseTimer]);

  if (visibleErrors.length === 0) {
    return null;
  }

  const dialog =
    isOpen && position && typeof document !== "undefined"
      ? createPortal(
          <div
            id={getFieldErrorDialogId(fieldName)}
            ref={dialogRef}
            className="form-field-error-dialog"
            role="tooltip"
            aria-live="polite"
            data-placement={position.placement}
            style={
              {
                top: position.top,
                left: position.left,
                width: position.width,
                "--validation-dialog-arrow-left": `${position.arrowLeft}px`,
              } as CSSProperties
            }
            onMouseEnter={clearCloseTimer}
            onMouseLeave={closeDialogSoon}
          >
            <div
              className="form-field-error-dialog-marker"
              aria-hidden="true"
            />

            <div className="form-field-error-dialog-content">
              {insights.map((insight) => (
                <article
                  key={`${insight.message}-${insight.recommendedFix}`}
                  className="form-validation-card"
                  data-severity="fatal"
                >
                  <div className="form-validation-card-meta">
                    <span>Validation issue</span>
                    <small>{toFriendlyFieldName(fieldName)}</small>
                  </div>

                  <strong>{insight.message}</strong>

                  <p>{insight.businessContext}</p>

                  <div className="form-validation-card-guidance">
                    <small>Recommended action</small>
                    <p>{insight.recommendedFix}</p>
                  </div>
                </article>
              ))}
            </div>
          </div>,
          document.body,
        )
      : null;

  return (
    <>
      <span
        ref={anchorRef}
        className="form-field-error-anchor"
        aria-hidden="true"
      />
      {dialog}
    </>
  );
}
