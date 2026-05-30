export type PromptStatus = "Pending" | "Processing" | "Completed" | "Failed";

export interface PromptJob {
  id: string;
  prompt: string;
  status: PromptStatus;
  result: string | null;
  errorMessage: string | null;
  attemptCount: number;
  createdAtUtc: string;
  processingStartedAtUtc: string | null;
  processingFinishedAtUtc: string | null;
  updatedAtUtc: string | null;
}

export interface CreatePromptsRequest {
  prompts: string[];
}

export interface ApiFieldError {
  field: string;
  code: string;
  message: string;
}

export interface ApiErrorResponse {
  code: string;
  message: string;
  errors: ApiFieldError[];
  traceId: string;
}
