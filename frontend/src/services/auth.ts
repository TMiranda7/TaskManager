import { API_BASE_URL } from '@/env';

export type LoginRequest = {
  username: string;
  password: string;
};

export type LoginResponse = {
  token: string;
  refreshToken: string;
  expiresTime: string;
};

async function postAuth<T>(path: string, body: unknown): Promise<T> {
  let response: Response;

  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    });
  } catch {
    throw new Error(
      `Não foi possível conectar à API (${API_BASE_URL}). Verifique se o backend está rodando e se o celular está na mesma rede Wi-Fi do computador.`,
    );
  }

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || 'Erro ao autenticar.');
  }

  return response.json() as Promise<T>;
}

export async function login(request: LoginRequest): Promise<LoginResponse> {
  return postAuth<LoginResponse>('/api/Auth/login', request);
}

export async function refreshToken(refreshTokenValue: string): Promise<LoginResponse> {
  return postAuth<LoginResponse>('/api/Auth/refresh', {
    refreshToken: refreshTokenValue,
  });
}
