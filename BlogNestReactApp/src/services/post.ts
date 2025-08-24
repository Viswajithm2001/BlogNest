// src/services/post.ts
import api from "./api";
import { type User } from "./user";
/* DTOs that match PostResponseDto / CommentResponseDto from backend */
export interface CommentResponse {
  id: string;
  content: string;
  createdAt: string;
  postId: string;
  userId: string;
  authorUsername?: string | null;
}
export interface CreatePostDto {
  title: string;
  content: string;
  tags?: string[];
}

export interface Post {
  id: string;
  title: string;
  content: string;
  createdAt: string;
  userId: string;
  user: User;               // backend returns full user object
  authorUsername: string;
  isAuthorPublic: boolean;
  tags: Tag[];            // backend returns tags as string[]
  comments?: CommentResponse[];
  imageUrl?: string;
}
export interface Tag{
    id: string;
    name: string;
}
export interface PaginatedPosts {
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  posts: Post[];
}
export const getPosts = async () => {
  try {
    const token = localStorage.getItem("token"); // get token from login
    const response = await api.get("/post", {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err: any) {
    console.error("Error fetching posts:", err.response || err.message);
    throw err;
  }
};
/* Get paginated posts (matches GET /api/post) */
export async function getAllPosts(
  page = 1,
  pageSize = 10,
  search?: string
): Promise<PaginatedPosts> {
  const resp = await api.get("/post", { params: { page, pageSize, search } });
  return resp.data;
}

/* Get single post (GET /api/post/{id}) */
export async function getPostById(id: string): Promise<Post> {
  const token = localStorage.getItem("token"); // get stored token
  if (!token) throw new Error("Not authenticated");

  const resp = await api.get(`/post/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`, // add token
    },
  });

  return resp.data;
}

/* Get posts by user (GET /api/post/user/{accountId}) */
export async function getPostsByUser(accountId: string): Promise<Post[]> {
  const resp = await api.get(`/post/user/${accountId}`);
  return resp.data;
}

/* Get posts by tag (GET /api/post/tag/{tagName}) */
export async function getPostsByTag(tag: string): Promise<Post[]> {
  const resp = await api.get(`/post/tag/${encodeURIComponent(tag)}`);
  return resp.data;
}

/* Create post (POST /api/post) — backend expects JSON CreatePostDto */
export const createPost = async (data: CreatePostDto) => {
  try {
    const token = localStorage.getItem("token"); // get token from login
    const response = await api.post("/post", data, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err: any) {
    console.error("Error creating post:", err.response || err.message);
    throw err;
  }
};

/* Upload image for an existing post (POST /api/post/{postId}/upload-image) */
export async function uploadPostImage(postId: string, file: File): Promise<{ imageUrl: string }> {
  const fd = new FormData();
  fd.append("image", file);
  const resp = await api.post(`/post/${postId}/upload-image`, fd, {
    headers: { "Content-Type": "multipart/form-data" }
  });
  return resp.data;
}

/* Update post (PUT /api/post/{id}) — backend expects form-data */
export async function updatePost(
  id: string,
  title?: string,
  content?: string,
  tags?: string[],
  image?: File,
  deleteImage = false
): Promise<void> {
  const fd = new FormData();
  if (title) fd.append("title", title);
  if (content) fd.append("content", content);
  if (tags && tags.length > 0) tags.forEach(t => fd.append("tags", t));
  if (image) fd.append("image", image);
  if (deleteImage) fd.append("deleteImage", "true");

  await api.put(`/post/${id}`, fd, {
    headers: { "Content-Type": "multipart/form-data" }
  });
}

/* Delete post */
export async function deletePost(id: string): Promise<void> {
  await api.delete(`/post/${id}`);
}
