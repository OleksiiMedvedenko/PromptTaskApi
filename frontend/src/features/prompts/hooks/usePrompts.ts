import { useCallback, useEffect, useRef, useState } from "react";
import { getPrompts } from "../../../api/promptsApi";
import type { PromptJob } from "../types";

export function usePrompts(pollingIntervalMs = 2000) {
  const [data, setData] = useState<PromptJob[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isFetching, setIsFetching] = useState(false);
  const [error, setError] = useState<unknown>(null);
  const isRequestRunningRef = useRef(false);
  const abortControllerRef = useRef<AbortController | null>(null);

  const refetch = useCallback(async () => {
    if (isRequestRunningRef.current) {
      return;
    }

    const abortController = new AbortController();
    abortControllerRef.current = abortController;
    isRequestRunningRef.current = true;
    setIsFetching(true);

    try {
      const prompts = await getPrompts(abortController.signal);
      setData(prompts);
      setError(null);
    } catch (requestError) {
      if (
        requestError instanceof DOMException &&
        requestError.name === "AbortError"
      ) {
        return;
      }

      setError(requestError);
    } finally {
      isRequestRunningRef.current = false;
      abortControllerRef.current = null;
      setIsLoading(false);
      setIsFetching(false);
    }
  }, []);

  useEffect(() => {
    void refetch();
    const intervalId = window.setInterval(
      () => void refetch(),
      pollingIntervalMs,
    );

    return () => {
      window.clearInterval(intervalId);
      abortControllerRef.current?.abort();
    };
  }, [pollingIntervalMs, refetch]);

  return {
    data,
    isLoading,
    isFetching,
    isError: error !== null,
    error,
    refetch,
  };
}
