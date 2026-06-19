import InvoiceSummaryTable from "../components/InvoiceSummaryTable";

function OverviewPage() {
  return (
    <main className="min-h-screen bg-slate-50 px-6 py-8">
      <div className="mx-auto max-w-7xl">
        <header className="mb-6">
          <h1 className="text-2xl font-semibold text-slate-900">
            Invoice Overview
          </h1>

          <p className="mt-1 text-sm text-slate-500">
            Review invoice audit records and processing statuses.
          </p>
        </header>

        <InvoiceSummaryTable />
      </div>
    </main>
  );
}

export default OverviewPage;