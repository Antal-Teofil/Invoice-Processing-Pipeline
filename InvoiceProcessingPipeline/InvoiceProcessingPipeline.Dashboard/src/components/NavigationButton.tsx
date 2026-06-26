import { Link } from "react-router-dom";

type NavigationButtonProperties = {
  displayText: string;
  to: string;
};

export default function NavigationButton({
  displayText,
  to,
}: NavigationButtonProperties) {
  return (
    <Link to={to} className="navigation-button">
      <span
        aria-hidden="true"
        className="navigation-button-shadow"
      />

      <span
        aria-hidden="true"
        className="navigation-button-edge"
      />

      <span className="navigation-button-front">
        {displayText}
      </span>
    </Link>
  );
}