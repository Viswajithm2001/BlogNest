import React from "react";
import { Link } from "react-router-dom";

const SamplePosts = [
  { id: 1, title: "First Post", content: "This is the content of the first post." },
  { id: 2, title: "Second Post", content: "This is the content of the second post." }
];
const Posts:React.FC = () =>{
    return (
        <div>
            <h1 className="text-2x1 font-bold mb-4">All Posts</h1>
            <ul className="space-y-3">
                {SamplePosts.map((post) => (
                    <li key={post.id} className="border p-4 rounded-lg">
                        <h2 className="text-xl font-semibold">{post.title}</h2>
                        <p className="mt-2">{post.content}</p>
                        <Link to={`/posts/${post.id}`} className="text-blue-500 hover:underline mt-2 inline-block">Read More</Link>
                    </li>
                ))}
            </ul>
        </div>
    )
}
export default Posts;