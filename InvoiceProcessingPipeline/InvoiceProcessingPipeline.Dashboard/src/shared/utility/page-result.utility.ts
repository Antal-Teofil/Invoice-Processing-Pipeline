export type PagedResult<T>  = {
    items: Array<T>;
    continuationToken: string | null;
    isAny: boolean;
};