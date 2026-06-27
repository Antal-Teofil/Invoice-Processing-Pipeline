import { create } from "zustand";

type InvoiceSummaryRecordTableStore = {
  pageSize: number;
  currentPageIndex: number;

  setPageSize: (pageSize: number) => void;
  setCurrentPageIndex: (pageIndex: number) => void;
  goToPreviousPage: () => void;
};

export const useInvoiceSummaryRecordTableStore =
  create<InvoiceSummaryRecordTableStore>()((set) => ({
    pageSize: 20,
    currentPageIndex: 0,

    setPageSize: (pageSize) =>
      set({
        pageSize,
        currentPageIndex: 0,
      }),

    setCurrentPageIndex: (pageIndex) =>
      set({
        currentPageIndex: Math.max(0, pageIndex),
      }),

    goToPreviousPage: () =>
      set((state) => ({
        currentPageIndex: Math.max(0, state.currentPageIndex - 1),
      })),
  }));