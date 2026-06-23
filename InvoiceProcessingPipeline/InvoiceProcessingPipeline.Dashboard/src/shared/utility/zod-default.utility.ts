import { z } from "zod";

type SafeDefault<T> = Exclude<T, undefined>;

export function objectDefault<TSchema extends z.ZodTypeAny>(
  schema: TSchema
) {
  const createDefault = (): SafeDefault<z.output<TSchema>> => {
    return schema.parse({}) as SafeDefault<z.output<TSchema>>;
  };

  return z
    .preprocess(
      (value) => value ?? createDefault(),
      schema
    )
    .default(createDefault);
}

export function arrayDefault<TSchema extends z.ZodTypeAny>(
  schema: TSchema
) {
  const createDefault = (): z.output<TSchema>[] => [];

  return z
    .preprocess(
      (value) => value ?? createDefault(),
      z.array(schema)
    )
    .default(createDefault);
}