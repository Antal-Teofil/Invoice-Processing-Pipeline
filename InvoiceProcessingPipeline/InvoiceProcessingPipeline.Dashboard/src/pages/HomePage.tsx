import InvoiceTable from "../components/InvoiceTable";
import { type InvoiceRow } from "../components/InvoiceTableRow";

export default function HomePage() {
  const invoiceRows: InvoiceRow[] = [
    {
      invoiceId: "INV-001",
      vendorName: "Acme Ltd.",
      emailAddress: "billing@acme.com",
      phoneNumber: "+36 30 123 4567",
      amount: 245.5,
      issueDate: "2025-04-01",
      status: "Approved",
    },
    {
      invoiceId: "INV-002",
      vendorName: "BlueSoft Kft.",
      emailAddress: "finance@bluesoft.hu",
      phoneNumber: "+36 20 555 1122",
      amount: 1299.99,
      issueDate: "2025-04-03",
      status: "Under Review",
    },
    {
      invoiceId: "INV-003",
      vendorName: "Nova Supply",
      emailAddress: "accounts@novasupply.com",
      phoneNumber: "+36 70 888 2211",
      amount: 89.0,
      issueDate: "2025-04-05",
      status: "Correction Required",
    },
    {
      invoiceId: "INV-004",
      vendorName: "Pixel Works",
      emailAddress: "office@pixelworks.io",
      phoneNumber: "+36 30 777 9988",
      amount: 560.75,
      issueDate: "2025-04-06",
      status: "Cancelled",
    },
    {
      invoiceId: "INV-005",
      vendorName: "GreenTech Zrt.",
      emailAddress: "payments@greentech.hu",
      phoneNumber: "+36 1 234 5678",
      amount: 3200.0,
      issueDate: "2025-04-08",
      status: "Approved",
    },
  ];

  return <InvoiceTable invoiceRows={invoiceRows} />;
}