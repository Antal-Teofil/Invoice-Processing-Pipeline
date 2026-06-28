export function scrollToSection(sectionName: string | null) {
  if (!sectionName) return;

  const element = document.getElementById(sectionName);

  if (!element) return;

  element.scrollIntoView({
    behavior: "smooth",
    block: "center",
  });

  element.classList.remove("field-highlight");

  void element.offsetWidth;

  element.classList.add("field-highlight");

  element.addEventListener(
    "animationend",
    () => {
      element.classList.remove("field-highlight");
    },
    { once: true }
  );
}