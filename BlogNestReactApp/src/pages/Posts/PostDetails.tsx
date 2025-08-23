import React, { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { getPostById, type Post } from "../../services/post";
import { createComment, type Comment } from "../../services/comment";
import { useAuth } from "../../context/AuthContext";

const PostDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [post, setPost] = useState<Post | null>(null);
    const [comment, setComment] = useState("");
    const [error, setError] = useState<string | null>(null);
    const { user } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!id) return;

        const fetchData = async () => {
            try {
                const data = await getPostById(id);
                setPost(data);
            } catch (err: any) {
                setError("Failed to load post details");
                console.log(err.message);
            }
        };
        fetchData();
    }, [id]);

    if (error) return <p className="text-red-500">{error}</p>;
    if (!post) return <p>Loading...</p>;

    const isAuthor = user === post.authorUsername;

    const handleAddComment = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!comment.trim() || !id) return;

        try {
            const newComment: Comment = await createComment({ content: comment, postId: id });
            setPost({
                ...post,
                comments: [newComment, ...(post.comments ?? [])],
            });
            setComment("");
        } catch (err: any) {
            console.error("Failed to add comment", err);
            alert("Error posting comment");
        }
    };

    return (
        <div className="max-w-2xl mt-10 p-6 bg-white shadow rounded">
            <h1 className="text-2xl font-bold mb-4">{post.title}</h1>
            <p className="mb-4">{post.content}</p>

            {post.imageUrl && (
                <img
                    src={post.imageUrl}
                    alt="Post"
                    className="mb-4 max-h-80 w-full object-cover rounded"
                />
            )}

            {/* Tags */}
            {post.tags && post.tags.length > 0 && (
                <div className="mb-4">
                    <strong>Tags:</strong>{" "}
                    {post.tags.map((tag, i) => (
                        <span
                            key={i}
                            className="mr-2 px-2 py-1 bg-gray-200 rounded"
                        >
                            {tag}
                        </span>
                    ))}
                </div>
            )}

            {/* Comments */}
            <div className="mt-8">
                <h2 className="text-xl font-semibold mb-4">Comments</h2>
                {post.comments && post.comments.length > 0 ? (
                    <ul className="space-y-4">
                        {post.comments.map((c) => (
                            <li key={c.id} className="border p-3 rounded bg-gray-50">
                                <p className="text-gray-800">{c.content}</p>
                                <small className="text-gray-500">
                                    By {c.authorUsername ?? "Anonymous"} on{" "}
                                    {new Date(c.createdAt).toLocaleString()}
                                </small>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <p className="text-gray-500">No comments yet.</p>
                )}

                {/* Add Comment Form */}
                <form onSubmit={handleAddComment} className="mt-6">
                    <textarea
                        value={comment}
                        onChange={(e) => setComment(e.target.value)}
                        placeholder="Write a comment..."
                        className="w-full p-2 border rounded-lg focus:outline-none focus:ring focus:ring-blue-300"
                    />
                    <button
                        type="submit"
                        className="mt-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
                    >
                        Add Comment
                    </button>
                </form>
            </div>

            <p className="mb-4 text-gray-600">Author: {post.authorUsername}</p>

            {isAuthor && (
                <button
                    onClick={() => navigate(`/posts/${post.id}/edit`)}
                    className="bg-blue-500 text-white px-4 py-2 rounded mb-4"
                >
                    Edit Post
                </button>
            )}

            <Link
                to="/posts"
                className="mt-4 inline-block text-blue-600 hover:underline"
            >
                ← Back to posts
            </Link>
        </div>
    );
};

export default PostDetails;
