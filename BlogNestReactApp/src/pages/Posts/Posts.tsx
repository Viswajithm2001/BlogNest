import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getAllPosts, type PaginatedPosts, type Post } from "../../services/post";

const Posts: React.FC = () => {
  const [paginatedPosts , setPosts] = useState<PaginatedPosts | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const data = await getAllPosts();
        setPosts(data);
      } catch (err: any) {
        setError("Failed to load posts");
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, []);

  if (loading) return <p>Loading posts...</p>;
  if (error) return <p className="text-red-500">{error}</p>;

  return (
    <div className="max-w-3xl mx-auto mt-8">
      <h1 className="text-2xl font-bold mb-4">Posts</h1>
      <div className="space-y-4">
        {paginatedPosts?.posts.map((post) => (
          <div key={post.id} className="border p-4 rounded shadow">
            {post.imageUrl && (
              <img
                src={post.imageUrl}
                alt={post.title}
                className="mb-2 w-full h-48 object-cover rounded"
              />
            )}
            <h2 className="text-xl font-semibold">{post.title}</h2>
            <p className="text-gray-700">
              {post.content.length > 150
                ? post.content.slice(0, 150) + "..."
                : post.content}
            </p>
            <div className="mt-2 flex flex-wrap">
              {post.tags.map((tag) => (
                <span
                  key={tag.id}
                  className="mr-2 mb-1 px-2 py-1 bg-gray-200 rounded"
                >
                  {tag.name}
                </span>
              ))}
            </div>
            <p className="text-sm text-gray-500 mt-2">
              By {post.authorUsername} •{" "}
              {new Date(post.createdAt).toLocaleDateString()}
            </p>
            <Link
              to={`/posts/${post.id}`}
              className="text-blue-600 hover:underline mt-2 inline-block"
            >
              Read more →
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Posts;
