import { axiosClient } from "../clients/AxiosClient";
import { DocumentRecordMetadataSchema, type DocumentRecordMetadata } from "../dtos/DocumentRecordMetadata";
import {z} from "zod";
import type { InvoiceSummaryCard } from "../types/InvoiceSummaryCard";

export async function fetchPagedInvoiceRecords(pageSize) {
    
    // lekerjuk a recordokat, kapunk egy valaszt
    const { data } = await axiosClient.get<DocumentRecordMetadata[]>("audit/records?pageSize=");
    
    // parsoljuk a beerkezett objektum tombot
    const result = await z.array(DocumentRecordMetadataSchema).safeParseAsync(data);

    const convertedResult = result.map((s) => );

    // amennyiben nem megfelelo formatumuak/tipusuak, akkor hibat dobunk
    if(!result.success) {
        console.error(result.error.issues);
        throw new Error("Nem sikerult parsolni a DocumentRecordMetadata objektumokat!");
    }

    // visszateritunk egy page-t
    return result.data;
};

function ConvertToInvoiceSummaryCard(record : DocumentRecordMetadata) : InvoiceSummaryCard {
    return {
        invoiceId: record.invoiceId,
        vendor: record.vendorName,
        phoneNumber: record.phoneNumber,
        vendorEmailAddress: record.vendorEmailAddress,
        totalAmount: record.totalAmount,
        currencyCode: record.currencyCode,
        status: record.auditStatus
    };
};