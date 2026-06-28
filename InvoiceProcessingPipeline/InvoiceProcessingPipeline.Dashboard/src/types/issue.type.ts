import z from "zod";
import type { IssueShema } from "../schemas/issue.schema";

export type IssueType = z.infer<typeof IssueShema>;