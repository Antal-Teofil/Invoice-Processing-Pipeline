import { create } from "zustand";

type IssueStore = {
  isOpen: boolean;
  openDrawer: () => void;
  closeDrawer: () => void;
  setOpen: (open: boolean) => void;
};

export const useIssueStore = create<IssueStore>()((set) => ({
  isOpen: false,

  openDrawer: () => set({ isOpen: true }),

  closeDrawer: () => set({ isOpen: false }),

  setOpen: (open) => set({ isOpen: open }),
}));