import { en } from "./locales/en";
import { pl } from "./locales/pl";

export type Language = "pl" | "en";

export const supportedLanguages: {
  code: Language;
  label: string;
  name: string;
}[] = [
  { code: "pl", label: "PL", name: "Polski" },
  { code: "en", label: "EN", name: "English" },
];

export type TranslationValue = string | Record<string, string>;
export type TranslationDictionary = Record<string, TranslationValue>;

export const translations: Record<Language, TranslationDictionary> = {
  pl,
  en,
};
