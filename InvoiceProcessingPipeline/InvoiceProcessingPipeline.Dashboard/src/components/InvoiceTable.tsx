import InvoiceTableRow, { type InvoiceRow } from "./InvoiceTableRow";

type InvoiceTableProperties = {
  invoiceRows: InvoiceRow[];
};

export default function InvoiceTable({ invoiceRows }: InvoiceTableProperties) {
  return (
    <section className="overflow-hidden rounded-[28px] border border-white/60 bg-white/75 shadow-[0_20px_80px_rgba(15,23,42,0.18)] backdrop-blur-xl">
      <div className="flex items-center justify-between border-b border-slate-200/70 bg-white/40 px-6 py-5">
        <div>
          <h2 className="text-2xl font-bold text-slate-900">Invoices</h2>
          <p className="mt-1 text-sm text-slate-500">
            Latest vendor invoices and payment details
          </p>
        </div>

        <div className="whitespace-nowrap rounded-full border border-slate-200 bg-white px-5 py-2.5 text-sm font-medium text-slate-600 shadow-sm">
          {invoiceRows.length} records
        </div>
      </div>

      <div className="w-full">
        <table className="w-full table-fixed border-collapse">
          <thead className="bg-slate-50/80">
            <tr>
              <th className="w-[11%] px-4 py-4 text-left text-[11px] font-bold uppercase tracking-[0.18em] text-slate-500">
                Invoice ID
              </th>
              <th className="w-[14%] px-4 py-4 text-left text-[11px] font-bold uppercase tracking-[0.18em] text-slate-500">
                Vendor
              </th>
              <th className="w-[22%] px-4 py-4 text-left text-[11px] font-bold uppercase tracking-[0.18em] text-slate-500">
                Email
              </th>
              <th className="w-[16%] px-4 py-4 text-left text-[11px] font-bold uppercase tracking-[0.18em] text-slate-500">
                Phone
              </th>
              <th className="w-[10%] px-4 py-4 text-left text-[11px] font-bold uppercase tracking-[0.18em] text-slate-500">
                Amount
              </th>
              <th className="w-[11%] px-4 py-4 text-left text-[11px] font-bold uppercase tracking-[0.18em] text-slate-500">
                Issue Date
              </th>
              <th className="w-[16%] px-4 py-4 text-left text-[11px] font-bold uppercase tracking-[0.18em] text-slate-500">
                Status
              </th>
            </tr>
          </thead>

          <tbody>
            {invoiceRows.map((row) => (
              <InvoiceTableRow key={row.invoiceId} {...row} />
            ))}
          </tbody>
        </table>
      </div>
    </section>
  );
}