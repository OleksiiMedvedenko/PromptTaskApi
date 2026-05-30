import type { ApiErrorResponse } from "../../features/prompts/types";

export function extractApiErrorCode(error: unknown): string | undefined {
  if (error && typeof error === "object") {
    const apiError = error as Partial<ApiErrorResponse>;
    return apiError.errors?.[0]?.code ?? apiError.code;
  }

  return undefined;
}
