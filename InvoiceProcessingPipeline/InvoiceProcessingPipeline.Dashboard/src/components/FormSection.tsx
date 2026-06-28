import type { ReactNode } from "react";

type FormSectionProps = {
  id: string;
  title: string;
  children: ReactNode;
};

export function FormSection({ id, title, children }: FormSectionProps) {
  return (
    <section id={id} className="invoice-editor-section">
      <h2>{title}</h2>

      {children}
    </section>
  );
}