import NavigationButton from "../components/NavigationButton";

function HomePage() {
  return (
    <main className="flex min-h-screen items-center justify-center bg-slate-50 px-6 py-16">
      <section className="w-full max-w-3xl rounded-3xl border border-slate-200 bg-white px-10 py-14 text-center shadow-sm sm:px-14 sm:py-16">
        <span className="mb-6 inline-flex rounded-full bg-sky-50 px-4 py-1.5 text-sm font-medium text-sky-700 ring-1 ring-inset ring-sky-600/20">
          Invoryx
        </span>

        <h1 className="mx-auto max-w-2xl text-4xl font-semibold tracking-tight text-slate-900 sm:text-5xl">
          Foreign Invoice Processing. Audit Ready.
        </h1>

        <div className="mx-auto mt-8 max-w-2xl space-y-5 text-base leading-8 text-slate-600">
          <p>
            Invoryx automatically collects and processes foreign invoices,
            turning incoming documents into structured, review-ready data.
          </p>

          <p>
            Invite your team, track every change, and keep your international
            invoice workflow secure, organized, and fully auditable.
          </p>

          <p className="pt-2 font-semibold text-slate-900">
            Less manual work. Clearer records. Greater control.
          </p>
        </div>

        <div className="mt-10 flex justify-center">
          <NavigationButton
            displayText="Explore Dashboard →"
            to="/dashboard"
          />
        </div>
      </section>
    </main>
  );
}

export default HomePage;