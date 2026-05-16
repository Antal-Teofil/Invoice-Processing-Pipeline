
export type Severity = "info" | "warning" | "error";

export type Issue = {
    severity: Severity;
    rule: string;
    message: string | null;
    fieldName: string;
};