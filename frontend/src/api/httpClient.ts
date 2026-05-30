const apiBaseUrl =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5000/api";

export async function requestJson<TResponse>(
  path: string,
  init?: RequestInit,
): Promise<TResponse> {
  const response = await fetch(`${apiBaseUrl}${path}`, {
    ...init,
    headers: {
      "Content-Type": "application/json",
      ...(init?.headers ?? {}),
    },
  });

  const contentType = response.headers.get("content-type") ?? "";
  const payload = contentType.includes("application/json")
    ? await response.json()
    : undefined;

  if (!response.ok) {
    throw payload ?? { code: "UNEXPECTED_ERROR", message: response.statusText };
  }

  return payload as TResponse;
}
