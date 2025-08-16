import axios from "axios";
import { getToken } from "./storage";

const api = axios.create({
  baseURL: "http://localhost:5075/api"
});

api.interceptors.request.use((config) => {
  const token = getToken();
  if(token) {
     config.headers = config.headers ?? {}; 
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
})

export default api;