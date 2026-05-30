import { useI18n } from "./I18nProvider";
import { supportedLanguages, type Language } from "./translations";

export function LanguageSwitcher() {
  const { language, setLanguage, t } = useI18n();

  return (
    <label className="language-select" aria-label={t("language.switcherLabel")}>
      <span>{t("language.shortLabel")}</span>
      <select
        value={language}
        onChange={(event) => setLanguage(event.target.value as Language)}
      >
        {supportedLanguages.map((item) => (
          <option key={item.code} value={item.code}>
            {item.name}
          </option>
        ))}
      </select>
    </label>
  );
}
