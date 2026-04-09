export type PaginatedResponse<T> = {
    items: Array<T>;
    continuationToken: string | null;
    hasMore: boolean;
};