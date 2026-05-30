import { useState } from "react";
import { createPrompts } from "../../../api/promptsApi";
import type { CreatePromptsRequest } from "../types";

export function useCreatePrompts(onSuccess?: () => void) {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<unknown>(null);

  const mutateAsync = async (request: CreatePromptsRequest) => {
    setIsPending(true);
    setError(null);
    try {
      const response = await createPrompts(request);
      onSuccess?.();
      return response;
    } catch (requestError) {
      setError(requestError);
      throw requestError;
    } finally {
      setIsPending(false);
    }
  };

  return {
    mutateAsync,
    isPending,
    isError: error !== null,
    error,
    reset: () => setError(null),
  };
}
