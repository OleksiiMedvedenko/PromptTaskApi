import type { TranslationDictionary } from "../translations";

export const pl: TranslationDictionary = {
  "app.productName": "PromptTaskSystem",
  "app.shortName": "Panel promptów",
  "app.tagline": "Proste zarządzanie kolejką",
  "app.title": "Wyślij prompty i sprawdzaj, co już zostało przetworzone",
  "app.description":
    "Dodawaj pojedyncze prompty albo wklej kilka naraz. System zapisze je w bazie, a worker przetworzy zadania w tle.",
  "app.noteTitle": "Co tu widzisz?",
  "app.noteText":
    "Po lewej przygotowujesz paczkę promptów. Po prawej widzisz statusy, wyniki i błędy bez ręcznego odświeżania strony.",

  "language.switcherLabel": "Język interfejsu",
  "language.shortLabel": "Język",

  "form.kicker": "Nowa paczka",
  "form.title": "Przygotuj prompty",
  "form.description":
    "Wpisz jedno zadanie albo wklej kilka pozycji naraz. Najpierw dodaj je do paczki, a potem wyślij całość do przetworzenia.",
  "form.label": "Co chcesz przetworzyć?",
  "form.placeholder":
    "Np. Popraw mój tekst\nNp. Streść ten dokument\nNp. Wygeneruj pomysły",
  "form.hint": "Możesz wkleić kilka pozycji, każdą w osobnej linii.",
  "form.addToQueue": "Dodaj do paczki",
  "form.detectedPrompts": "Wykryto w polu: {{count}}",
  "form.queueLabel": "Prompty przygotowane do wysłania",
  "form.queueTitle": "Do wysłania: {{count}}",
  "form.clearQueue": "Wyczyść",
  "form.removePrompt": "Usuń prompt",
  "form.submitHint": "Po wysłaniu zadania trafią do kolejki workera.",
  "form.submit": "Wyślij",
  "form.submitting": "Wysyłanie...",

  "list.kicker": "Statusy",
  "list.title": "Prompty w systemie",
  "list.description":
    "Lista odświeża się automatycznie. Najnowsze zadania są widoczne jako pierwsze.",
  "list.loading": "Ładowanie listy promptów...",
  "list.loadError": "Nie udało się pobrać promptów:",
  "list.pollingActive": "Odświeżanie włączone",
  "list.statusSummaryLabel": "Podsumowanie statusów",
  "list.all": "Wszystkie",
  "list.emptyTitle": "Brak promptów",
  "list.emptyDescription": "Przygotuj i wyślij pierwszą paczkę promptów.",
  "list.attempt": "Próba {{count}}",
  "list.createdAt": "Dodano {{date}}, {{time}}",
  "list.lastSubmitted": "Ostatnio wysłano: {{date}}, {{time}}",
  "list.resultTitle": "Wynik",
  "list.errorTitle": "Błąd",

  "pagination.previous": "Poprzednia",
  "pagination.next": "Następna",
  "pagination.page": "{{current}} / {{total}}",
  "pagination.range": "{{start}}–{{end}} z {{total}}",
  "common.emptyValue": "—",

  status: {
    Pending: "Oczekuje",
    Processing: "Przetwarzane",
    Completed: "Zakończone",
    Failed: "Nieudane",
  },

  error: {
    VALIDATION_FAILED: "Formularz zawiera błędy.",
    PROMPTS_REQUIRED: "Dodaj przynajmniej jeden prompt.",
    PROMPTS_LIMIT_EXCEEDED: "Możesz wysłać maksymalnie 50 promptów naraz.",
    PROMPT_REQUIRED: "Prompt nie może być pusty.",
    PROMPT_TOO_LONG: "Prompt może mieć maksymalnie 4000 znaków.",
    PROMPT_NOT_FOUND: "Nie znaleziono promptu.",
    UNEXPECTED_ERROR: "Wystąpił nieoczekiwany błąd serwera.",
    UNKNOWN_ERROR: "Wystąpił nieznany błąd.",
    OPERATION_FAILED: "Nie udało się wykonać operacji. Spróbuj ponownie.",
  },
};
