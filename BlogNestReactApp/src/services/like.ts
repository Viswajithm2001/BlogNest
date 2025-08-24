import api from "./api";

export const likePost = async (postId: string) => {
  const token = localStorage.getItem("token");
  const resp = await api.post(`/likes/like/${postId}`, null, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return resp.data;
};

export const unlikePost = async (postId: string) => {
  const token = localStorage.getItem("token");
  const resp = await api.delete(`/likes/unlike/${postId}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return resp.data;
};

export const getLikeCount = async (postId: string) => {
  const resp = await api.get(`/likes/count/${postId}`);
  return resp.data.likeCount;
};

export const isPostLikedByUser = async (postId: string): Promise<{ liked: boolean }> => {
  const token = localStorage.getItem("token");
  const resp = await api.get(`/likes/user-liked/${postId}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return resp.data; // { liked: true/false }
};