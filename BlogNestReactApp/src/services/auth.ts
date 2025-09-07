import axios from "axios";
import api from "./api";
export interface AuthResponse {
  token: string;
  username:string;
    // user: {
    //     id: string;
    //     username: string;
    //     email: string;
    // };
}
export interface ResetPassword{
  newPassword: string,
  confirmPassword:string,
  email: string
}
export async function Login(username: string, password: string): Promise<AuthResponse> {
  const response = await api.post<AuthResponse>("/auth/login", { username, password });
  return response.data;
}

export async function resetpwd(resetPassword: ResetPassword) {
  if (resetPassword.newPassword !== resetPassword.confirmPassword) {
    alert("Passwords do not match");
    return;
}
  const response = await api.put("/auth/reset-password", resetPassword);
  return response.data
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