import type {
  CreatePromptsRequest,
  PromptJob,
} from "../features/prompts/types";
import { requestJson } from "./httpClient";

export function getPrompts(signal?: AbortSignal): Promise<PromptJob[]> {
  return requestJson<PromptJob[]>("/prompts", { signal });
}

export function createPrompts(
  request: CreatePromptsRequest,
): Promise<PromptJob[]> {
  return requestJson<PromptJob[]>("/prompts/batch", {
    method: "POST",
    body: JSON.stringify(request),
  });
}
