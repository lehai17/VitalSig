import { createContext, useContext, useEffect, useMemo, useState } from "react";
import { getCurrentUser, login as loginRequest, register as registerRequest } from "../api/auth";

const TOKEN_KEY = "vitalsig-token";
const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [token, setToken] = useState(() => localStorage.getItem(TOKEN_KEY));
  const [currentUser, setCurrentUser] = useState(null);
  const [bootstrapped, setBootstrapped] = useState(false);

  useEffect(() => {
    async function bootstrap() {
      if (!token) {
        setBootstrapped(true);
        return;
      }

      try {
        const user = await getCurrentUser(token);
        setCurrentUser(user);
      } catch {
        localStorage.removeItem(TOKEN_KEY);
        setToken(null);
        setCurrentUser(null);
      } finally {
        setBootstrapped(true);
      }
    }

    bootstrap();
  }, [token]);

  async function login(credentials) {
    const response = await loginRequest(credentials);
    localStorage.setItem(TOKEN_KEY, response.token);
    setToken(response.token);
    setCurrentUser(response.user);
  }

  async function register(payload) {
    const response = await registerRequest(payload);
    localStorage.setItem(TOKEN_KEY, response.token);
    setToken(response.token);
    setCurrentUser(response.user);
  }

  function logout() {
    localStorage.removeItem(TOKEN_KEY);
    setToken(null);
    setCurrentUser(null);
  }

  const value = useMemo(
    () => ({ token, currentUser, bootstrapped, login, register, logout }),
    [bootstrapped, currentUser, token]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider.");
  }

  return context;
}
