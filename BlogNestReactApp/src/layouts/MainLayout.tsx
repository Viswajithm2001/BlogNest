import React from "react";
import { Outlet, useNavigate } from "react-router-dom";
import Navbar from "../components/Navbar";
import { useAuth } from "../context/AuthContext";

const MainLayout: React.FC = () => {
  const navigate = useNavigate();

  // 🔹 Mock user state
  const userlogged = {
    profilePictureUrl: "https://i.pravatar.cc/40",
  };
  const {user, logout } = useAuth();
  const isLoggedIn = !!user; // change to false to see guest navbar

  const handleLogout = () => {
    console.log("Logging out...");
    // 🔹 later: clear token / context
    logout();
    navigate("/login");
  };

  return (
    <div className="flex flex-col min-h-screen">
      <Navbar
      />
      <main className="container mx-auto px-4 py-8 flex-1">
        <Outlet />
      </main>
      <footer className="bg-gray-100 text-gray-700 py-4">
        <div className="container mx-auto px-4 text-center">
          © {new Date().getFullYear()} BlogNest. All rights reserved.
        </div>
      </footer>
    </div>
  );
};

export default MainLayout;
