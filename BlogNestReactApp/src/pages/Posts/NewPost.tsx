// src/pages/Post/NewPost.tsx
import React from "react";
import CreatePostForm from "./CreatePostForm";

const NewPost: React.FC = () => {
  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-50">
      <div className="w-full max-w-2xl bg-white shadow-md rounded-lg p-6">
        <h1 className="text-2xl font-bold text-indigo-600 mb-6 text-center">
          Create New Post
        </h1>
        <CreatePostForm />
      </div>
    </div>
  );
};

export default NewPost;
