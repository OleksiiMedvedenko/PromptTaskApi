import { useEffect, useMemo, useState } from "react";
import { useI18n } from "../../../i18n/I18nProvider";
import { Card } from "../../../shared/components/Card";
import { extractApiErrorCode } from "../../../shared/errors/errorMessages";
import type { PromptJob, PromptStatus } from "../types";
import { PromptStatusBadge } from "./PromptStatusBadge";

const statusOrder: PromptStatus[] = [
  "Pending",
  "Processing",
  "Completed",
  "Failed",
];
const pageSize = 5;

interface PromptListProps {
  lastSubmittedAt: Date | null;
  promptsQuery: {
    data: PromptJob[];
    isLoading: boolean;
    isFetching: boolean;
    isError: boolean;
    error: unknown;
  };
}

function getStatusCount(prompts: PromptJob[], status: PromptStatus) {
  return prompts.filter((prompt) => prompt.status === status).length;
}

function formatDateTime(
  value: string | null,
  locale: string,
  emptyValue: string,
) {
  if (!value) {
    return { date: emptyValue, time: emptyValue };
  }

  const date = new Date(value);

  return {
    date: new Intl.DateTimeFormat(locale, {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    }).format(date),
    time: new Intl.DateTimeFormat(locale, {
      hour: "2-digit",
      minute: "2-digit",
    }).format(date),
  };
}

function getSortedPrompts(prompts: PromptJob[]) {
  return [...prompts].sort(
    (a, b) =>
      new Date(b.createdAtUtc).getTime() - new Date(a.createdAtUtc).getTime(),
  );
}

interface PaginationControlsProps {
  currentPage: number;
  totalPages: number;
  startItem: number;
  endItem: number;
  totalItems: number;
  onPageChange: (page: number) => void;
}

function PaginationControls({
  currentPage,
  totalPages,
  startItem,
  endItem,
  totalItems,
  onPageChange,
}: PaginationControlsProps) {
  const { t } = useI18n();

  if (totalItems <= pageSize) {
    return null;
  }

  return (
    <div className="pagination-bar">
      <span>
        {t("pagination.range", {
          start: startItem,
          end: endItem,
          total: totalItems,
        })}
      </span>
      <div className="pagination-actions">
        <button
          type="button"
          disabled={currentPage === 1}
          onClick={() => onPageChange(currentPage - 1)}
        >
          {t("pagination.previous")}
        </button>
        <strong>
          {t("pagination.page", { current: currentPage, total: totalPages })}
        </strong>
        <button
          type="button"
          disabled={currentPage === totalPages}
          onClick={() => onPageChange(currentPage + 1)}
        >
          {t("pagination.next")}
        </button>
      </div>
    </div>
  );
}

export function PromptList({ lastSubmittedAt, promptsQuery }: PromptListProps) {
  const { t, getStatusLabel, dateLocale, getErrorMessage } = useI18n();
  const { data, isLoading, isError, error, isFetching } = promptsQuery;
  const [page, setPage] = useState(1);
  const prompts = useMemo(() => getSortedPrompts(data ?? []), [data]);
  const totalPages = Math.max(1, Math.ceil(prompts.length / pageSize));
  const currentPage = Math.min(page, totalPages);
  const startIndex = (currentPage - 1) * pageSize;
  const visiblePrompts = prompts.slice(startIndex, startIndex + pageSize);
  const startItem = prompts.length === 0 ? 0 : startIndex + 1;
  const endItem = Math.min(startIndex + pageSize, prompts.length);

  useEffect(() => {
    if (page > totalPages) {
      setPage(totalPages);
    }
  }, [page, totalPages]);

  if (isLoading) {
    return <Card className="queue-card loading-card">{t("list.loading")}</Card>;
  }

  if (isError) {
    const errorCode = extractApiErrorCode(error);
    const message = errorCode
      ? getErrorMessage(errorCode)
      : t("errors.unexpected");

    return (
      <Card className="queue-card error-card">
        {t("list.loadError")} {message}
      </Card>
    );
  }

  return (
    <Card className="queue-card">
      <div className="card-header queue-header">
        <div>
          <span className="section-label">{t("list.kicker")}</span>
          <h2>{t("list.title")}</h2>
          <p>{t("list.description")}</p>
        </div>
        <span
          className={`polling-indicator ${isFetching ? "is-fetching" : ""}`}
        >
          {t("list.pollingActive")}
        </span>
      </div>

      <div className="status-summary" aria-label={t("list.statusSummaryLabel")}>
        <div className="summary-tile total">
          <span>{t("list.all")}</span>
          <strong>{prompts.length}</strong>
        </div>
        {statusOrder.map((status) => (
          <div
            className={`summary-tile tile-${status.toLowerCase()}`}
            key={status}
          >
            <span>{getStatusLabel(status)}</span>
            <strong>{getStatusCount(prompts, status)}</strong>
          </div>
        ))}
      </div>

      {lastSubmittedAt &&
        (() => {
          const submittedAt = formatDateTime(
            lastSubmittedAt.toISOString(),
            dateLocale,
            t("common.emptyValue"),
          );

          return (
            <div className="last-submit-note">
              {t("list.lastSubmitted", {
                date: submittedAt.date,
                time: submittedAt.time,
              })}
            </div>
          );
        })()}

      {prompts.length === 0 ? (
        <div className="empty-state">
          <strong>{t("list.emptyTitle")}</strong>
          <span>{t("list.emptyDescription")}</span>
        </div>
      ) : (
        <>
          <PaginationControls
            currentPage={currentPage}
            totalPages={totalPages}
            startItem={startItem}
            endItem={endItem}
            totalItems={prompts.length}
            onPageChange={setPage}
          />

          <div className="prompt-list">
            {visiblePrompts.map((job) => {
              const createdAt = formatDateTime(
                job.createdAtUtc,
                dateLocale,
                t("common.emptyValue"),
              );

              return (
                <article className="prompt-item" key={job.id}>
                  <div className="prompt-item-header">
                    <PromptStatusBadge status={job.status} />
                    <div className="job-meta">
                      <span>
                        {t("list.attempt", { count: job.attemptCount })}
                      </span>
                      <span>
                        {t("list.createdAt", {
                          date: createdAt.date,
                          time: createdAt.time,
                        })}
                      </span>
                    </div>
                  </div>

                  <p className="prompt-text">{job.prompt}</p>

                  {job.result && (
                    <div className="result-box">
                      <strong>{t("list.resultTitle")}</strong>
                      <p>{job.result}</p>
                    </div>
                  )}

                  {job.errorMessage && (
                    <div className="error-box">
                      <strong>{t("list.errorTitle")}</strong>
                      <p>{job.errorMessage}</p>
                    </div>
                  )}
                </article>
              );
            })}
          </div>

          <PaginationControls
            currentPage={currentPage}
            totalPages={totalPages}
            startItem={startItem}
            endItem={endItem}
            totalItems={prompts.length}
            onPageChange={setPage}
          />
        </>
      )}
    </Card>
  );
}
