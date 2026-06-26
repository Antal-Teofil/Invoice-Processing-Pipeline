import NavigationButton from "../components/NavigationButton";

function HomePage() {
  return (
    <main className="home-page">
      <section className="home-card">
        <span className="home-brand-badge">
          Invoryx
        </span>

        <h1 className="home-title">
          Foreign Invoice Processing. Audit Ready.
        </h1>

        <div className="home-description">
          <p>
            Invoryx automatically collects and processes foreign invoices,
            turning incoming documents into structured, review-ready data.
          </p>

          <p>
            Invite your team, track every change, and keep your international
            invoice workflow secure, organized, and fully auditable.
          </p>

          <p className="home-tagline">
            Less manual work. Clearer records. Greater control.
          </p>
        </div>

        <div className="home-action">
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