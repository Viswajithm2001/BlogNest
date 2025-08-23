// src/services/comment.ts
import api from "./api";
import type { Post } from "./post";
export interface Comment {
  id: string;
  content: string;
  createdAt: string;
  postId: string;
  userId: string;
  authorUsername?: string | null;
}

export interface CreateCommentDto {
  postId: string;
  content: string;
}

export async function getCommentsByPost(postId: string): Promise<Comment[]> {
    const token = localStorage.getItem("token");
    console.log(postId);
  const resp = await api.get(`/post/${postId}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return resp.data;
}

export async function createComment(data: CreateCommentDto): Promise<Comment> {
    const token = localStorage.getItem("token");
  const resp = await api.post("/comment", data, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return resp.data;
}

export async function deleteComment(commentId: string): Promise<void> {
      const token = localStorage.getItem("token");
  await api.delete(`/comment/${commentId}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
}

export async function updateComment(commentId: string, content: string): Promise<Comment> {
    const token = localStorage.getItem("token");
  const resp = await api.put(
    `/comment/${commentId}`,
    { content },
    { headers: { Authorization: `Bearer ${token}` } }
  );
  return resp.data;
}
