import { createContext, useContext, useState } from "react";
import { Login, type AuthResponse } from "../services/auth";
import { setToken, clearToken } from "../services/storage";

type AuthState = {
  user: AuthResponse["user"] | null;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
};

const AuthContext = createContext<AuthState | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<AuthResponse["user"] | null>(null);

  const login = async (username: string, password: string) => {
    const { token, user } = await Login(username, password); // call API
    setToken(token);                                         // save token
    setUser(user);                                           // update state
  };

  const logout = () => {
    clearToken();  // remove token
    setUser(null); // reset state
  };

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
}
