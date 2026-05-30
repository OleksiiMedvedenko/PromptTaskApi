import { useMemo, useState } from "react";
import { useI18n } from "../../../i18n/I18nProvider";
import { Button } from "../../../shared/components/Button";
import { extractApiErrorCode } from "../../../shared/errors/errorMessages";
import { useCreatePrompts } from "../hooks/useCreatePrompts";
import {
  MAX_PROMPT_LENGTH,
  MAX_PROMPTS_PER_BATCH,
  parsePrompts,
} from "../schemas/promptFormSchema";

interface PromptFormProps {
  onSubmitted: () => void;
}

function removeAtIndex(items: string[], indexToRemove: number) {
  return items.filter((_, index) => index !== indexToRemove);
}

export function PromptForm({ onSubmitted }: PromptFormProps) {
  const { t, getErrorMessage } = useI18n();
  const [promptText, setPromptText] = useState("");
  const [queuedPrompts, setQueuedPrompts] = useState<string[]>([]);
  const [validationErrorCode, setValidationErrorCode] = useState<string | null>(
    null,
  );
  const createPrompts = useCreatePrompts(onSubmitted);

  const promptsFromText = useMemo(() => parsePrompts(promptText), [promptText]);
  const totalPrompts = queuedPrompts.length + promptsFromText.length;
  const apiErrorCode = createPrompts.isError
    ? extractApiErrorCode(createPrompts.error)
    : undefined;

  const validateBatch = (prompts: string[]) => {
    if (prompts.length === 0) {
      return "PROMPTS_REQUIRED";
    }

    if (prompts.length > MAX_PROMPTS_PER_BATCH) {
      return "PROMPTS_LIMIT_EXCEEDED";
    }

    if (prompts.some((prompt) => prompt.length > MAX_PROMPT_LENGTH)) {
      return "PROMPT_TOO_LONG";
    }

    return null;
  };

  const resetErrors = () => {
    setValidationErrorCode(null);
    createPrompts.reset();
  };

  const handleAddToQueue = () => {
    resetErrors();

    const nextBatch = [...queuedPrompts, ...promptsFromText];
    const errorCode = validateBatch(nextBatch);

    if (errorCode) {
      setValidationErrorCode(errorCode);
      return;
    }

    setQueuedPrompts(nextBatch);
    setPromptText("");
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    resetErrors();

    const promptsToSubmit = [...queuedPrompts, ...promptsFromText];
    const errorCode = validateBatch(promptsToSubmit);

    if (errorCode) {
      setValidationErrorCode(errorCode);
      return;
    }

    await createPrompts.mutateAsync({ prompts: promptsToSubmit });
    setQueuedPrompts([]);
    setPromptText("");
  };

  return (
    <form className="card prompt-form" onSubmit={handleSubmit}>
      <div className="card-header compact">
        <div>
          <span className="section-label">{t("form.kicker")}</span>
          <h2>{t("form.title")}</h2>
          <p>{t("form.description")}</p>
        </div>
        <span className="counter-pill">{totalPrompts}/50</span>
      </div>

      <label className="field-label" htmlFor="promptText">
        {t("form.label")}
      </label>
      <textarea
        id="promptText"
        value={promptText}
        rows={7}
        placeholder={t("form.placeholder")}
        onChange={(event) => {
          setPromptText(event.target.value);
          resetErrors();
        }}
      />

      <div className="prompt-compose-actions">
        <button
          className="secondary-button"
          disabled={promptsFromText.length === 0 || createPrompts.isPending}
          type="button"
          onClick={handleAddToQueue}
        >
          {t("form.addToQueue")}
        </button>
        <span className="hint-text">
          {promptsFromText.length > 0
            ? t("form.detectedPrompts", { count: promptsFromText.length })
            : t("form.hint")}
        </span>
      </div>

      {queuedPrompts.length > 0 && (
        <section className="draft-prompts" aria-label={t("form.queueLabel")}>
          <div className="draft-header">
            <strong>
              {t("form.queueTitle", { count: queuedPrompts.length })}
            </strong>
            <button type="button" onClick={() => setQueuedPrompts([])}>
              {t("form.clearQueue")}
            </button>
          </div>
          <div className="draft-list">
            {queuedPrompts.map((prompt, index) => (
              <div className="draft-item" key={`${prompt}-${index}`}>
                <span>{prompt}</span>
                <button
                  aria-label={t("form.removePrompt")}
                  type="button"
                  onClick={() =>
                    setQueuedPrompts((current) => removeAtIndex(current, index))
                  }
                >
                  ×
                </button>
              </div>
            ))}
          </div>
        </section>
      )}

      {validationErrorCode && (
        <p className="form-error">{getErrorMessage(validationErrorCode)}</p>
      )}
      {createPrompts.isError && (
        <p className="form-error">
          {getErrorMessage(apiErrorCode ?? "OPERATION_FAILED")}
        </p>
      )}

      <div className="form-actions">
        <span className="hint-text">{t("form.submitHint")}</span>
        <div className="submit-group">
          <span className="submit-count">{totalPrompts}/50</span>
          <Button
            disabled={createPrompts.isPending || totalPrompts === 0}
            type="submit"
          >
            {createPrompts.isPending ? t("form.submitting") : t("form.submit")}
          </Button>
        </div>
      </div>
    </form>
  );
}
