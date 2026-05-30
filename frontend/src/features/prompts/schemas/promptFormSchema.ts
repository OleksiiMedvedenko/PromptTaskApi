export const MAX_PROMPTS_PER_BATCH = 50;
export const MAX_PROMPT_LENGTH = 4000;

export function parsePrompts(value: string) {
  return value
    .split("\n")
    .map((prompt) => prompt.trim())
    .filter(Boolean);
}

export function validatePromptsText(value: string): string | null {
  const prompts = parsePrompts(value);

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
}
