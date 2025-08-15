import React from 'react';
import { BrowserRouter, Route, Routes} from "react-router-dom";
import MainLayout from "./layouts/MainLayout";
import Home from "./pages/Home/Home";
import Posts from "./pages/Posts/Posts";
import PostDetails from "./pages/Posts/PostDetails";
import Login from "./pages/Login/Login";
import Register from "./pages/Register/Register"

const App:React.FC = () => {
  return (
    <BrowserRouter>
    <Routes>
      <Route path='/' element = {<MainLayout />}>
      <Route index element={<Home />} />
      <Route path='home' element={<Home />} />
      <Route path='login' element={<Login />} />
      <Route path='register' element={<Register />} />
      <Route path='posts' element={<Posts />} />
      <Route path='posts/:id' element={<PostDetails />} />
      {/* Add more routes as needed */}
    </Route>
    </Routes>
    </BrowserRouter>
  )
}

export default App;