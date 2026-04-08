import InvoiceTable from "../components/InvoiceTable";

export default function HomePage() {
    return (
        <>
            <p>Hello Bello</p>
            <InvoiceTable invoiceRows={[]}></InvoiceTable>
        </>
    );
}