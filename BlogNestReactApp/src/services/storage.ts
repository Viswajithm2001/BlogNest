const TOKEN_KEY = "blogNestToken";

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function setToken(token: string): void {
  localStorage.setItem(TOKEN_KEY, token);
}

// 4) Remove the token on logout
export function clearToken(): void {
  localStorage.removeItem(TOKEN_KEY);
}