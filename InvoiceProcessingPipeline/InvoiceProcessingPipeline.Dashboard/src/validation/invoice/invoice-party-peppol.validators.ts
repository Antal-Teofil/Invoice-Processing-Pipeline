import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
} from "./shared/validator.types";
import { addFieldError, createFieldErrorBag } from "./shared/field-error-bag";
import { hasValue } from "./shared/value.utils";
import {
  hasAnyTaxIdentifier,
  hasLegalRegistrationIdentifier,
  hasVatIdentifier,
  isExemptTaxCategory,
  isExportTaxCategory,
  isIntraCommunityTaxCategory,
  isOutOfScopeTaxCategory,
  isReverseChargeTaxCategory,
  isStandardRatedTaxCategory,
  isZeroRatedTaxCategory,
} from "./shared/peppol-party.utils";

type TaxCategoryReference = {
  id: unknown;
  path: string;
};

function collectTaxCategoryReferences(invoice: CommercialInvoiceValidationModel) {
  const references: TaxCategoryReference[] = [];

  invoice.invoiceLine?.forEach((line, lineIndex) => {
    references.push({
      id: line.item?.classifiedTaxCategory?.id,
      path: `invoiceLine[${lineIndex}].item.classifiedTaxCategory.id`,
    });

    line.allowanceCharge?.forEach((allowanceCharge, allowanceChargeIndex) => {
      references.push({
        id: allowanceCharge.taxCategory?.id,
        path: `invoiceLine[${lineIndex}].allowanceCharge[${allowanceChargeIndex}].taxCategory.id`,
      });
    });
  });

  invoice.allowanceCharge?.forEach((allowanceCharge, allowanceChargeIndex) => {
    references.push({
      id: allowanceCharge.taxCategory?.id,
      path: `allowanceCharge[${allowanceChargeIndex}].taxCategory.id`,
    });
  });

  invoice.taxTotal?.forEach((taxTotal, taxTotalIndex) => {
    taxTotal.taxSubtotal?.forEach((taxSubtotal, taxSubtotalIndex) => {
      references.push({
        id: taxSubtotal.taxCategory?.id,
        path: `taxTotal[${taxTotalIndex}].taxSubtotal[${taxSubtotalIndex}].taxCategory.id`,
      });
    });
  });

  return references;
}

function hasAnyCategory(
  references: TaxCategoryReference[],
  predicate: (value: unknown) => boolean,
) {
  return references.some((reference) => predicate(reference.id));
}

function addFirstCategoryDrivenError(
  errors: FieldErrorBag,
  references: TaxCategoryReference[],
  predicate: (value: unknown) => boolean,
  targetPath: string,
  message: string,
) {
  const firstReference = references.find((reference) => predicate(reference.id));

  if (!firstReference) {
    return;
  }

  addFieldError(errors, targetPath, `${message} This is required by a related tax category.`);
}

export function validateInvoicePartyPeppolRules(
  invoice: CommercialInvoiceValidationModel,
): FieldErrorBag {
  const errors = createFieldErrorBag();
  const supplier = invoice.accountingSupplierParty;
  const customer = invoice.accountingCustomerParty;
  const taxCategories = collectTaxCategoryReferences(invoice);

  const supplierHasVat = hasVatIdentifier(supplier);
  const supplierHasAnyTaxId = hasAnyTaxIdentifier(supplier);
  const customerHasVat = hasVatIdentifier(customer);
  const customerHasLegalRegistration = hasLegalRegistrationIdentifier(customer);

  const hasStandardRated = hasAnyCategory(taxCategories, isStandardRatedTaxCategory);
  const hasZeroRated = hasAnyCategory(taxCategories, isZeroRatedTaxCategory);
  const hasExempt = hasAnyCategory(taxCategories, isExemptTaxCategory);
  const hasReverseCharge = hasAnyCategory(taxCategories, isReverseChargeTaxCategory);
  const hasIntraCommunity = hasAnyCategory(taxCategories, isIntraCommunityTaxCategory);
  const hasExport = hasAnyCategory(taxCategories, isExportTaxCategory);
  const hasOutOfScope = hasAnyCategory(taxCategories, isOutOfScopeTaxCategory);
  const populatedTaxCategories = taxCategories.filter((reference) => hasValue(reference.id));
  const hasOnlyOutOfScopeTaxCategories = populatedTaxCategories.length > 0 && populatedTaxCategories.every((reference) => isOutOfScopeTaxCategory(reference.id));

  if ((hasStandardRated || hasZeroRated || hasExempt) && !supplierHasVat && !supplierHasAnyTaxId) {
    addFirstCategoryDrivenError(
      errors,
      taxCategories,
      (id) => isStandardRatedTaxCategory(id) || isZeroRatedTaxCategory(id) || isExemptTaxCategory(id),
      "accountingSupplierParty.partyTaxScheme",
      "Seller VAT identifier or Seller tax registration identifier is required for Standard rated, Zero rated or Exempt VAT categories.",
    );
  }

  if (hasReverseCharge) {
    if (!supplierHasVat && !supplierHasAnyTaxId) {
      addFirstCategoryDrivenError(
        errors,
        taxCategories,
        isReverseChargeTaxCategory,
        "accountingSupplierParty.partyTaxScheme",
        "Seller VAT identifier or Seller tax registration identifier is required for Reverse charge VAT categories.",
      );
    }

    if (!customerHasVat && !customerHasLegalRegistration) {
      addFirstCategoryDrivenError(
        errors,
        taxCategories,
        isReverseChargeTaxCategory,
        "accountingCustomerParty.partyTaxScheme",
        "Buyer VAT identifier or Buyer legal registration identifier is required for Reverse charge VAT categories.",
      );
    }
  }

  if (hasIntraCommunity) {
    if (!supplierHasVat) {
      addFirstCategoryDrivenError(
        errors,
        taxCategories,
        isIntraCommunityTaxCategory,
        "accountingSupplierParty.partyTaxScheme",
        "Seller VAT identifier is required for Intra-community supply VAT categories.",
      );
    }

    if (!customerHasVat) {
      addFirstCategoryDrivenError(
        errors,
        taxCategories,
        isIntraCommunityTaxCategory,
        "accountingCustomerParty.partyTaxScheme",
        "Buyer VAT identifier is required for Intra-community supply VAT categories.",
      );
    }
  }

  if (hasExport && !supplierHasVat) {
    addFirstCategoryDrivenError(
      errors,
      taxCategories,
      isExportTaxCategory,
      "accountingSupplierParty.partyTaxScheme",
      "Seller VAT identifier is required for Export outside the EU VAT categories.",
    );
  }

  if (hasOutOfScope && hasOnlyOutOfScopeTaxCategories && (supplierHasVat || customerHasVat)) {
    addFirstCategoryDrivenError(
      errors,
      taxCategories,
      isOutOfScopeTaxCategory,
      supplierHasVat ? "accountingSupplierParty.partyTaxScheme" : "accountingCustomerParty.partyTaxScheme",
      "Seller VAT identifier and Buyer VAT identifier must not be present when the invoice only uses Not subject to VAT category rules.",
    );
  }

  return errors;
}
