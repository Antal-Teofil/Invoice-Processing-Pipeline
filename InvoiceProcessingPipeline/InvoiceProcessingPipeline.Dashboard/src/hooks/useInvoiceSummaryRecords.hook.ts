import { useCallback, useMemo } from "react";
import {
  useInfiniteQuery,
  type InfiniteData,
} from "@tanstack/react-query";
import { useInvoiceSummaryRecordTableStore } from "../stores/useInvoiceSummaryRecordTable.store";
import type { InvoiceSummaryRecord, PagedInvoiceSummaryRecordDTO } from "../types/invoice-summary-record";
import { fetchInvoiceSummaryRecordsAsync } from "../services/invoice-records.service";
import { mapInvoiceSummaryRecordDtoToTableRow } from "../mappers/invoice-summary-record.mapper";

type InvoiceSummaryRecordsQueryKey = readonly [
  "invoice-summary-records",
  {
    pageSize: number;
  }
];

export function useInvoiceSummaryRecords() {
  const pageSize = useInvoiceSummaryRecordTableStore(
    (state) => state.pageSize
  );

  const currentPageIndex = useInvoiceSummaryRecordTableStore(
    (state) => state.currentPageIndex
  );

  const setCurrentPageIndex = useInvoiceSummaryRecordTableStore(
    (state) => state.setCurrentPageIndex
  );

  const goToPreviousPage = useInvoiceSummaryRecordTableStore(
    (state) => state.goToPreviousPage
  );

  const query = useInfiniteQuery<
    PagedInvoiceSummaryRecordDTO,
    Error,
    InfiniteData<PagedInvoiceSummaryRecordDTO, string | null>,
    InvoiceSummaryRecordsQueryKey,
    string | null
  >({
    queryKey: ["invoice-summary-records", { pageSize }],
    initialPageParam: null,

    queryFn: ({ pageParam }) =>
      fetchInvoiceSummaryRecordsAsync({
        pageSize,
        continuationToken: pageParam,
      }),

    getNextPageParam: (lastPage) =>
      lastPage.continuationToken ?? undefined,

    retry: false,
    retryOnMount: false,
    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    staleTime: 10_000,
  });

  const pages = query.data?.pages ?? [];
  const currentPage = pages[currentPageIndex];

  const records: InvoiceSummaryRecord[] = useMemo(
    () =>
      currentPage?.items.map(mapInvoiceSummaryRecordDtoToTableRow) ?? [],
    [currentPage]
  );

  const canGoToPreviousPage = currentPageIndex > 0;

  const canGoToNextPage =
    currentPageIndex < pages.length - 1 || Boolean(query.hasNextPage);

  const goToNextPage = useCallback(async () => {
    const hasAlreadyLoadedNextPage = currentPageIndex < pages.length - 1;

    if (hasAlreadyLoadedNextPage) {
      setCurrentPageIndex(currentPageIndex + 1);
      return;
    }

    if (!query.hasNextPage || query.isFetchingNextPage) {
      return;
    }

    const previousPageCount = pages.length;
    const result = await query.fetchNextPage();
    const nextPageCount = result.data?.pages.length ?? previousPageCount;

    if (nextPageCount > previousPageCount) {
      setCurrentPageIndex(currentPageIndex + 1);
    }
  }, [
    currentPageIndex,
    pages.length,
    query.hasNextPage,
    query.isFetchingNextPage,
    query.fetchNextPage,
    setCurrentPageIndex,
  ]);

  return {
    records,

    pageSize,
    currentPageIndex,
    currentPageNumber: currentPageIndex + 1,

    isLoading: query.isLoading,
    isFetching: query.isFetching,
    isFetchingNextPage: query.isFetchingNextPage,
    error: query.error,

    canGoToPreviousPage,
    canGoToNextPage,

    goToPreviousPage,
    goToNextPage,
  };
}