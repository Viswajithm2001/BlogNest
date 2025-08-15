import React from "react";
import { Outlet } from "react-router-dom";
import Navbar from "../components/Navbar";

const MainLayout: React.FC = () => {
  return (
    <div className="flex flex-col min-h-screen">
      <Navbar />
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
