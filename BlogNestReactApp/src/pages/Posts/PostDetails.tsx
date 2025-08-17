import React, { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { getPostById, type Post } from "../../services/post";
import { useAuth } from "../../context/AuthContext";

const PostDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [post, setPost] = useState<Post | null>(null);
    const [error, setError] = useState<string | null>(null);
    const { user } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!id) return;
        const fetchPost = async () => {
            try {
                const data = await getPostById(id);
                setPost(data);
            } catch (err: any) {
                setError("Failed to load post details");
            }
        };
        fetchPost();
    }, [id]);

    if (error) return <p className="text-red-500">{error}</p>;
    if (!post) return <p>Loading...</p>;

    const isAuthor = user?.id === post.user.id;

    return (
        <div className="max-w-2xl mx-auto mt-10 p-6 bg-white shadow rounded">
            <h1 className="text-2xl font-bold mb-4">{post.title}</h1>
            <p className="mb-4">{post.content}</p>

            {post.imageUrl && (
                <img src={post.imageUrl} alt="Post" className="mb-4 max-h-80 w-full object-cover rounded" />
            )}

            {post.tags.length > 0 && (
                <div className="mb-4">
                    <strong>Tags:</strong>{" "}
                    {post.tags.map(tag => (
                        <span key={tag.id} className="mr-2 px-2 py-1 bg-gray-200 rounded">
                            {tag.name}
                        </span>
                    ))}
                </div>
            )}

            <p className="mb-4 text-gray-600">Author: {post.user.username}</p>

            {isAuthor && (
                <button
                    onClick={() => navigate(`/posts/${post.id}/edit`)}
                    className="bg-blue-500 text-white px-4 py-2 rounded mb-4"
                >
                    Edit Post
                </button>
            )}

            <Link to="/posts" className="mt-4 inline-block text-blue-600 hover:underline">
                ← Back to posts
            </Link>
        </div>
    );
};

export default PostDetails;
