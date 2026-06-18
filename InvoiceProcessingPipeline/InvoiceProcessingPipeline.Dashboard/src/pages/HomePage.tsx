import NavigationButton from "../components/NavigationButton";

function HomePage() {
    return (
        <>
            <section>
                <h1>Foreign Invoice Processing. Audit Ready.</h1>

                <p>
                    Invoryx automatically collects and processes foreign invoices, turning
                    incoming documents into structured, review-ready data.
                </p>

                <p>
                    Invite your team, track every change, and keep your international invoice workflow secure, organized, and fully auditable.
                </p>

                <p>
                    <strong>Less manual work. Clearer records. Greater control.</strong>
                </p>
            </section>

            <NavigationButton displayText="Explore Dashboard →" to="/dashboard" />
        </>
    );
}

export default HomePage;