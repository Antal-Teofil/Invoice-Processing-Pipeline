import { z } from 'zod';
import type { DocumentMetadataDTOScheme, DocumentSummaryRecordScheme } from '../scheme/invoice-schemes';
import type { InvoiceScheme } from '../scheme/InvoiceScheme';

export type InvoiceDocumentSummaryDTO = z.infer<typeof DocumentMetadataDTOScheme>;
export type InvoiceSummaryRecord = z.infer<typeof DocumentSummaryRecordScheme>;
export type Invoice = z.infer<typeof InvoiceScheme>;