import { API_BASE_URL } from '@/env';
import { refreshToken as refreshTokenRequest } from './auth';
import { clearTokens, getRefreshToken, getToken, saveTokens } from './storage';

type SessionExpiredHandler = () => void;

let onSessionExpired: SessionExpiredHandler | null = null;

export function setSessionExpiredHandler(handler: SessionExpiredHandler | null): void {
  onSessionExpired = handler;
}

async function parseError(response: Response): Promise<string> {
  const text = await response.text();

  try {
    const parsed = JSON.parse(text) as { message?: string };
    if (parsed?.message) {
      return parsed.message;
    }
  } catch {
  }

  return text || 'Erro ao comunicar com o backend.';
}

async function refreshSession(): Promise<string | null> {
  const storedRefreshToken = await getRefreshToken();
  if (!storedRefreshToken) {
    return null;
  }

  try {
    const refreshed = await refreshTokenRequest(storedRefreshToken);
    await saveTokens(refreshed.token, refreshed.refreshToken);
    return refreshed.token;
  } catch {
    await clearTokens();
    onSessionExpired?.();
    return null;
  }
}

async function request<T>(
  path: string,
  options?: RequestInit,
  authRequired = false,
  retried = false,
): Promise<T> {
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...(options?.headers as Record<string, string> | undefined),
  };

  if (authRequired) {
    const token = await getToken();
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }
  }

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers,
  });

  if (response.status === 401 && authRequired && !retried) {
    const newToken = await refreshSession();

    if (newToken) {
      return request<T>(path, options, authRequired, true);
    }
  }

  if (response.status === 401 && authRequired) {
    await clearTokens();
    onSessionExpired?.();
  }

  if (!response.ok) {
    throw new Error(await parseError(response));
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

export const api = {
  get: <T>(path: string, authRequired = false) => request<T>(path, { method: 'GET' }, authRequired),
  post: <T>(path: string, body: unknown, authRequired = false) =>
    request<T>(path, { method: 'POST', body: JSON.stringify(body) }, authRequired),
  put: <T>(path: string, body: unknown, authRequired = false) =>
    request<T>(path, { method: 'PUT', body: JSON.stringify(body) }, authRequired),
};
