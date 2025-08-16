import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../../services/api";
interface Post {
  id: number;
  title: string;
  content: string;
}

export default function Posts() {
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const response = await api.get<Post[]>("/post");
        setPosts(response.data);
      } catch (error) {
        console.error("Error fetching posts:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, []);

  if (loading) return <p>Loading posts...</p>;

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Posts</h1>
      {posts.length === 0 ? (
        <p>No posts available.</p>
      ) : (
        <ul className="space-y-3">
          {posts.map((post) => (
            <li key={post.id} className="border p-3 rounded-lg">
              <Link to={`/posts/${post.id}`} className="text-blue-500 hover:underline">
                {post.title}
              </Link>
              <p className="text-gray-600">{post.content.slice(0, 80)}...</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
