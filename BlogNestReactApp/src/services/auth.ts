import axios from "axios";
import api from "./api";

export interface AuthResponse {
  token: string;
    user: {
        id: number;
        username: string;
        email: string;
    };
}

export async function Login(username: string, password: string): Promise<AuthResponse> {
  const response = await api.post<AuthResponse>("/auth/login", { username, password });
  console.log("Login response:", response.data);
  return response.data;
}

const API_URL = api.defaults.baseURL;

export const register = async (username: string, email:string, password: string, isPublic: boolean) => {
  try {
    const response = await axios.post(`${API_URL}/auth/register`, {
      username,
      email,
      password,
      isPublic
    });
    return response.data; // usually returns success message or token
  } catch (error: any) {
    throw error;
  }
};