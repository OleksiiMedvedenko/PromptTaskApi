import type { TranslationDictionary } from "../translations";

export const en: TranslationDictionary = {
  "app.productName": "PromptTaskSystem",
  "app.shortName": "Prompt panel",
  "app.tagline": "Simple queue management",
  "app.title": "Submit prompts and check what has already been processed",
  "app.description":
    "Add a single prompt or paste several at once. The system stores them in the database and the worker processes them in the background.",
  "app.noteTitle": "What is shown here?",
  "app.noteText":
    "Prepare a batch on the left. On the right you can follow statuses, results, and errors without refreshing the page manually.",

  "language.switcherLabel": "Interface language",
  "language.shortLabel": "Language",

  "form.kicker": "New batch",
  "form.title": "Prepare prompts",
  "form.description":
    "Enter one task or paste several items at once. Add them to the batch first, then submit the whole batch for processing.",
  "form.label": "What should be processed?",
  "form.placeholder":
    "E.g. Improve my text\nE.g. Summarize this document\nE.g. Generate ideas",
  "form.hint": "You can paste several items, one per line.",
  "form.addToQueue": "Add to batch",
  "form.detectedPrompts": "Detected in the field: {{count}}",
  "form.queueLabel": "Prompts prepared for submission",
  "form.queueTitle": "Ready to submit: {{count}}",
  "form.clearQueue": "Clear",
  "form.removePrompt": "Remove prompt",
  "form.submitHint": "After submitting, items are added to the worker queue.",
  "form.submit": "Submit",
  "form.submitting": "Submitting...",

  "list.kicker": "Statuses",
  "list.title": "Prompts in the system",
  "list.description":
    "The list refreshes automatically. Newest jobs are shown first.",
  "list.loading": "Loading prompts...",
  "list.loadError": "Could not load prompts:",
  "list.pollingActive": "Refresh enabled",
  "list.statusSummaryLabel": "Status summary",
  "list.all": "All",
  "list.emptyTitle": "No prompts yet",
  "list.emptyDescription": "Prepare and submit the first prompt batch.",
  "list.attempt": "Attempt {{count}}",
  "list.createdAt": "Added {{date}}, {{time}}",
  "list.lastSubmitted": "Last submitted: {{date}}, {{time}}",
  "list.resultTitle": "Result",
  "list.errorTitle": "Error",

  "pagination.previous": "Previous",
  "pagination.next": "Next",
  "pagination.page": "{{current}} / {{total}}",
  "pagination.range": "{{start}}–{{end}} of {{total}}",
  "common.emptyValue": "—",

  status: {
    Pending: "Pending",
    Processing: "Processing",
    Completed: "Completed",
    Failed: "Failed",
  },

  error: {
    VALIDATION_FAILED: "The form contains validation errors.",
    PROMPTS_REQUIRED: "Add at least one prompt.",
    PROMPTS_LIMIT_EXCEEDED: "You can submit up to 50 prompts at once.",
    PROMPT_REQUIRED: "Prompt cannot be empty.",
    PROMPT_TOO_LONG: "Prompt can contain up to 4000 characters.",
    PROMPT_NOT_FOUND: "Prompt was not found.",
    UNEXPECTED_ERROR: "An unexpected server error occurred.",
    UNKNOWN_ERROR: "An unknown error occurred.",
    OPERATION_FAILED: "The operation could not be completed. Please try again.",
  },
};
