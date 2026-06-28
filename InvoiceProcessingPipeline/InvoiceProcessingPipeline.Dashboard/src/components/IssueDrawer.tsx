import { Dialog } from "radix-ui";

import type { IssueType } from "../types/issue.type";
import { useIssueStore } from "../stores/issue.store";
import { IssueCard } from "./IssueCard";
import { scrollToSection } from "../shared/utility/form.utility";

type IssueDrawerProps = {
  issues: IssueType[];
};

export function IssueDrawer({ issues }: IssueDrawerProps) {
  const isOpen = useIssueStore((state) => state.isOpen);
  const setOpen = useIssueStore((state) => state.setOpen);
  const closeDrawer = useIssueStore((state) => state.closeDrawer);

  if (!isOpen) {
    return null;
  }

  function handleIssueSelect(sectionName: string | null) {
    closeDrawer();
    scrollToSection(sectionName);
  }

  return (
    <Dialog.Root open={isOpen} onOpenChange={setOpen}>
      <Dialog.Portal>
        <div className="issue-drawer">
          <Dialog.Overlay data-issue-drawer-overlay />

          <Dialog.Content
            data-issue-drawer-content
            onMouseLeave={closeDrawer}
            onPointerDownOutside={(event) => {
              event.preventDefault();
            }}
            onEscapeKeyDown={(event) => {
              event.preventDefault();
            }}
          >
            <header>
              <div>
                <Dialog.Title>Invoice Issues</Dialog.Title>

                <Dialog.Description>
                  Click an issue to jump to the related form section.
                </Dialog.Description>
              </div>

              <Dialog.Close type="button">Close</Dialog.Close>
            </header>

            <div data-issue-drawer-list>
              {issues.length > 0 ? (
                issues.map((issue, index) => (
                  <IssueCard
                    key={`${issue.issueCode ?? "issue"}-${index}`}
                    issue={issue}
                    onSelect={handleIssueSelect}
                  />
                ))
              ) : (
                <p data-issue-drawer-empty>No issues found.</p>
              )}
            </div>
          </Dialog.Content>
        </div>
      </Dialog.Portal>
    </Dialog.Root>
  );
}