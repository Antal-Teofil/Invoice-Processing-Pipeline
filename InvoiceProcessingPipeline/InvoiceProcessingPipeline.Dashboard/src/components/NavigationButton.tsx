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
    <Link
      to={to}
      className="
        group relative inline-block cursor-pointer bg-transparent p-0
        transition-[filter] duration-[250ms]
        hover:brightness-110
      "
    >
      <span
        aria-hidden="true"
        className="
          absolute left-0 top-0 h-full w-full
          translate-y-[2px] rounded-lg bg-slate-400 blur-[2px]
          transition-transform duration-[600ms]
          ease-[cubic-bezier(0.3,0.7,0.4,1)]
          group-hover:translate-y-[4px]
          group-active:translate-y-[1px]
        "
      />

      <span
        aria-hidden="true"
        className="
          absolute left-0 top-0 h-full w-full rounded-lg
          bg-[linear-gradient(to_right,hsl(248,39%,39%)_0%,hsl(248,39%,49%)_8%,hsl(248,39%,39%)_92%,hsl(248,39%,29%)_100%)]
        "
      />

      <span
        className="
          relative block rounded-lg
          bg-gradient-to-r from-purple-800 via-purple-600 to-indigo-700
          px-8 py-4
          text-base font-semibold uppercase tracking-[1.5px] text-white
          -translate-y-1
          transition-transform duration-[600ms]
          ease-[cubic-bezier(0.3,0.7,0.4,1)]
          group-hover:-translate-y-[6px]
          group-active:-translate-y-0.5
        "
      >
        {displayText}
      </span>
    </Link>
  );
}