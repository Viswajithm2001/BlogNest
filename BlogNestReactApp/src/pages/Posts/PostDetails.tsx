import React from "react";
import { useParams, Link } from "react-router-dom";

const PostDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    return (
        <div>
            <h1 className="text-2x1 font-bold mb-4">Post Details</h1>
            <p> Showing details for the Post ID : {id}</p>
            <Link to="/posts" className="mt-4 inline-block text-blue-600 hover:underline">
                ← Back to posts
            </Link>
        </div>
    )
}

export default PostDetails;