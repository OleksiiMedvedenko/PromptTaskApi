import { useState } from "react";
import { PromptForm } from "./features/prompts/components/PromptForm";
import { PromptList } from "./features/prompts/components/PromptList";
import { usePrompts } from "./features/prompts/hooks/usePrompts";
import { useI18n } from "./i18n/I18nProvider";
import { LanguageSwitcher } from "./i18n/LanguageSwitcher";
import "./styles/app.css";

export function App() {
  const { t } = useI18n();
  const promptsQuery = usePrompts();
  const [lastSubmittedAt, setLastSubmittedAt] = useState<Date | null>(null);

  const handleSubmitted = () => {
    setLastSubmittedAt(new Date());
    void promptsQuery.refetch();
  };

  return (
    <main className="app-shell">
      <header className="top-bar">
        <div className="brand-lockup">
          <div className="brand-mark">PT</div>
          <div>
            <span>{t("app.productName")}</span>
            <strong>{t("app.shortName")}</strong>
          </div>
        </div>
        <LanguageSwitcher />
      </header>

      <section className="intro-panel">
        <div>
          <p className="section-label">{t("app.tagline")}</p>
          <h1>{t("app.title")}</h1>
          <p className="intro-text">{t("app.description")}</p>
        </div>
        <div className="intro-note">
          <strong>{t("app.noteTitle")}</strong>
          <span>{t("app.noteText")}</span>
        </div>
      </section>

      <div className="dashboard-grid">
        <PromptForm onSubmitted={handleSubmitted} />
        <PromptList
          lastSubmittedAt={lastSubmittedAt}
          promptsQuery={promptsQuery}
        />
      </div>
    </main>
  );
}
