# 🦸 BlogNest Cinematic Universe – Project Blueprint

## **1. Backend Saga** 🛠

### **Phase 1 – Origins: The Auth Arc**
- **Set up project & dependencies**  
  - ASP.NET Core Web API  
  - PostgreSQL + EF Core  
- **Database foundation**  
  - Create `User` entity & DbContext  
  - Apply migrations  
- **Authentication system**  
  - User registration endpoint (`/api/auth/register`)  
  - User login endpoint (`/api/auth/login`)  
  - JWT token generation with expiry  
- **Test with Postman**  
  - Verify registration  
  - Verify login & token generation  
  - Add token to Authorization header  
- **Security fixes**  
  - Ensure token changes per login  
  - Validate JWT in protected endpoints  

---

### **Phase 2 – Rise of the Posts**
- **Posts model & migration**  
  - Create `Post` entity  
  - Migrate database  
- **Posts CRUD API**  
  - `GET /api/posts/posts` – list all posts  
  - `GET /api/posts/{id}` – single post  
  - `POST /api/posts` – create post (Auth required)  
  - `PUT /api/posts/{id}` – update post (Auth required)  
  - `DELETE /api/posts/{id}` – delete post (Auth required)  
- **Role-based control (optional)**  
  - Admin vs normal user permissions  

---

### **Phase 3 – The Gauntlet of Extras**
- **Advanced features**  
  - Pagination & search in posts  
  - Image uploads for posts  
  - Comments system  
- **Production readiness**  
  - Error handling middleware  
  - Logging  
  - CORS setup for frontend  

---

## **2. Frontend Saga** 💻

### **Phase 1 – The React Awakens**
- **Project setup**  
  - Vite + React + TypeScript  
  - Tailwind CSS configuration  
- **Routing**  
  - React Router with `/`, `/login`, `/register`, `/posts`  
- **Auth integration**  
  - Login & register pages connected to backend  
  - Store JWT in `localStorage`  
  - Axios interceptor for adding Authorization header  

---

### **Phase 2 – Age of Posts**
- **Posts listing**  
  - Fetch & display all posts  
  - Show post details page  
- **Create/Edit post**  
  - Protected routes (check JWT)  
  - Forms for creating & updating posts  

---

### **Phase 3 – Infinity Polish**
- **UI/UX upgrades**  
  - Loading & error states  
  - Responsive layout  
  - Navbar with login/logout state  
- **Extras**  
  - User profile page  
  - Comment UI under posts  
  - Like/share buttons  

---

### **Phase 4 – Endgame**
- **Deployment**  
  - Deploy backend to Render/Railway  
  - Deploy frontend to Vercel/Netlify  
  - Configure CORS & environment variables  
- **Final QA**  
  - End-to-end testing  
  - Security checks  
---
## 🚀 Future Enhancements

The following features are planned for future versions of **BlogNest**:

- **🔑 Update Password**
  - Allow users to update/change their password after logging in.
  - Validate old password before setting new one.

- **📧 Forgot / Reset Password**
  - Implement "Forgot Password" flow using email/OTP or reset links.

- **🖼️ Profile Enhancements**
  - Allow updating profile details (bio, display name, etc.).
  - Add cover photos in addition to profile pictures.

- **👥 Follow System**
  - Users can follow/unfollow each other.
  - Show followers/following count on profile.

- **🔔 Notifications**
  - Real-time notifications for likes, comments, and new followers.

- **💬 Comments Enhancements**
  - Nested replies to comments.
  - Edit/delete comments.

- **❤️ Advanced Likes**
  - Support reactions (👍❤️😂 etc.) instead of just likes.

- **📊 Analytics**
  - Show post views, trending posts, and user activity insights.

- **🌐 Deployment**
  - Deploy the app on cloud platforms (Azure/AWS/Heroku/Netlify).

---

📌 These enhancements will be gradually introduced after completing the core features (authentication, posts, comments, likes, and basic profile management).
