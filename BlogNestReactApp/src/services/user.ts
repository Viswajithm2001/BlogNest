import api from "./api";

export interface User {
  id: string;
  username: string;
  email: string;
  profilePictureUrl?: string;
}

export async function getCurrentUser(): Promise<User> {
  const response = await api.get("/user/me");
  return response.data;
}

export async function updateProfilePicture(file: File): Promise<User> {
  const formData = new FormData();
  formData.append("profilePicture", file);

  const response = await api.post("/user/upload-profile-picture", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return response.data;
}

export async function updateProfile(updates: {
  username?: string;
  email?: string;
  isPublic?: boolean;
}): Promise<User> {
  const response = await api.put("/user/update-profile", updates);
  return response.data;
}
