// src/context/AuthContext.tsx
import { createContext, useContext, useState, useEffect } from "react";
import { Login as loginApi, type AuthResponse } from "../services/auth"; // import your auth.ts Login

type AuthContextType = {
  user: string | null;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType>({
  user: null,
  login: async () => {},
  logout: () => {},
});

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<string | null>(null);

  useEffect(() => {
    const storedUser = localStorage.getItem("username");
    if (storedUser) setUser(storedUser);
  }, []);

 async function login(username: string, password: string) {
  // Clear token first just in case
  localStorage.removeItem("token");
  localStorage.removeItem("username");
  
  const data: AuthResponse = await loginApi(username, password);
  localStorage.setItem("token", data.token);
  localStorage.setItem("username", data.username);
  setUser(data.username);
}


function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("username");
  setUser(null);
}

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
