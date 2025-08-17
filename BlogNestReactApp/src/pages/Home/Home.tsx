import React from "react";
import "./Home.css"; // Assuming you have some styles in Home.css
const Home: React.FC = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50 text-gray-800">
      <div className="bg-white shadow-md rounded-lg p-8 max-w-lg text-center">
        <h1 className="text-4xl font-extrabold text-indigo-600">
          Welcome to BlogNest
        </h1>
        <p className="mt-4 text-lg text-gray-600">
          Your go-to platform for sharing and reading posts.
        </p>
        <button className="mt-6 px-6 py-2 bg-indigo-600 text-white rounded-md shadow hover:bg-indigo-700 transition">
          Get Started
        </button>
      </div>
    </div>
  );
};

export default Home;
