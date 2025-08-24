import React from "react";
import { useNavigate } from "react-router-dom";
import NewPost from "../Posts/NewPost";
import "./Home.css";

const Home: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50 text-gray-800">
      <div className="bg-white shadow-md rounded-lg p-8 max-w-lg text-center">
        <h1 className="text-4xl font-extrabold text-indigo-600">
          Welcome to BlogNest
        </h1>
        <p className="mt-4 text-lg text-gray-600">
          Your go-to platform for sharing and reading posts.
        </p>

        <div className="mt-6 flex flex-col gap-4">
          <button
            onClick={() => navigate("/posts")}
            className="px-6 py-2 bg-indigo-600 text-white rounded-md shadow hover:bg-indigo-700 transition"
          >
            View Posts
          </button>

          <button
            onClick={() => navigate("/posts/new")}
            className="px-6 py-2 bg-emerald-600 text-white rounded-md shadow hover:bg-emerald-700 transition"
          >
            + New Post
          </button>
        </div>
      </div>
    </div>
  );
};

export default Home;

