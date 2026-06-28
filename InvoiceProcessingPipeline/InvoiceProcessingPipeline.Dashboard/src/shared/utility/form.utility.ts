export function scrollToSection(sectionName: string | null) {
  if (!sectionName) return;

  const element = document.getElementById(sectionName);

  if (!element) return;

  element.scrollIntoView({
    behavior: "smooth",
    block: "center",
  });

  element.animate(
    [
      {
        outline: "3px solid rgba(124, 58, 237, 0.65)",
        outlineOffset: "5px",
      },
      {
        outline: "3px solid rgba(124, 58, 237, 0)",
        outlineOffset: "5px",
      },
    ],
    {
      duration: 1200,
      easing: "ease-out",
    }
  );
}