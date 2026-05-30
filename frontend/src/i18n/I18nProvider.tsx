import {
  createContext,
  useContext,
  useMemo,
  useState,
  type PropsWithChildren,
} from "react";
import { translations, type Language } from "./translations";

type InterpolationValues = Record<string, string | number>;

interface I18nContextValue {
  language: Language;
  setLanguage: (language: Language) => void;
  t: (key: string, values?: InterpolationValues) => string;
  getStatusLabel: (status: string) => string;
  getErrorMessage: (code?: string) => string;
  dateLocale: string;
}

const I18nContext = createContext<I18nContextValue | null>(null);

function resolveBrowserLanguage(): Language {
  const storedLanguage = window.localStorage.getItem("prompt-task-language");

  if (storedLanguage === "pl" || storedLanguage === "en") {
    return storedLanguage;
  }

  return navigator.language.toLowerCase().startsWith("pl") ? "pl" : "en";
}

function interpolate(value: string, values?: InterpolationValues) {
  if (!values) {
    return value;
  }

  return Object.entries(values).reduce(
    (result, [key, replacement]) =>
      result.split(`{{${key}}}`).join(String(replacement)),
    value,
  );
}

function getNestedValue(source: unknown, path: string): string | undefined {
  return path.split(".").reduce<unknown>((current, segment) => {
    if (current && typeof current === "object" && segment in current) {
      return (current as Record<string, unknown>)[segment];
    }

    return undefined;
  }, source) as string | undefined;
}

export function I18nProvider({ children }: PropsWithChildren) {
  const [language, setLanguageState] = useState<Language>(() =>
    resolveBrowserLanguage(),
  );

  const value = useMemo<I18nContextValue>(() => {
    const dictionary = translations[language];

    const translate = (key: string, values?: InterpolationValues) => {
      const directTranslation = dictionary[key];
      const fallbackDirectTranslation = translations.en[key];
      const translation =
        (typeof directTranslation === "string"
          ? directTranslation
          : undefined) ??
        getNestedValue(dictionary, key) ??
        (typeof fallbackDirectTranslation === "string"
          ? fallbackDirectTranslation
          : undefined) ??
        getNestedValue(translations.en, key) ??
        key;

      return interpolate(translation, values);
    };

    const setLanguage = (nextLanguage: Language) => {
      window.localStorage.setItem("prompt-task-language", nextLanguage);
      setLanguageState(nextLanguage);
    };

    return {
      language,
      setLanguage,
      t: translate,
      getStatusLabel: (status: string) => translate(`status.${status}`),
      getErrorMessage: (code?: string) =>
        translate(`error.${code ?? "UNKNOWN_ERROR"}`),
      dateLocale: language === "pl" ? "pl-PL" : "en-US",
    };
  }, [language]);

  return <I18nContext.Provider value={value}>{children}</I18nContext.Provider>;
}

export function useI18n() {
  const context = useContext(I18nContext);

  if (!context) {
    throw new Error("useI18n must be used inside I18nProvider.");
  }

  return context;
}
