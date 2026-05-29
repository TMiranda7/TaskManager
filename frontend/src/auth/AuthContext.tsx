import { createContext, PropsWithChildren, useContext, useEffect, useMemo, useState } from 'react';
import { login as loginService } from '../services/auth';
import { setSessionExpiredHandler } from '../services/api';
import { clearTokens, getToken, saveTokens } from '../services/storage';

type AuthContextValue = {
  token: string | null;
  loading: boolean;
  signIn: (username: string, password: string) => Promise<void>;
  signOut: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: PropsWithChildren) {
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const bootstrap = async () => {
      const persistedToken = await getToken();
      setToken(persistedToken);
      setLoading(false);
    };

    bootstrap();
  }, []);

  useEffect(() => {
    setSessionExpiredHandler(() => {
      setToken(null);
    });

    return () => {
      setSessionExpiredHandler(null);
    };
  }, []);

  const signIn = async (username: string, password: string) => {
    const response = await loginService({ username, password });
    await saveTokens(response.token, response.refreshToken);
    setToken(response.token);
  };

  const signOut = async () => {
    await clearTokens();
    setToken(null);
  };

  const value = useMemo(
    () => ({
      token,
      loading,
      signIn,
      signOut,
    }),
    [token, loading],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth deve ser usado dentro de AuthProvider');
  }

  return context;
}
