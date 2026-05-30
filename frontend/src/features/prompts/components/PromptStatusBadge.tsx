import { useI18n } from "../../../i18n/I18nProvider";
import type { PromptStatus } from "../types";

export function PromptStatusBadge({ status }: { status: PromptStatus }) {
  const { getStatusLabel } = useI18n();

  return (
    <span className={`status-badge status-${status.toLowerCase()}`}>
      {getStatusLabel(status)}
    </span>
  );
}
