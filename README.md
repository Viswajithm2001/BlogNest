# 📝 BlogNest

![.NET Badge](https://img.shields.io/badge/.NET-6-blue)  
![React Badge](https://img.shields.io/badge/React-18-blue)  
![PostgreSQL Badge](https://img.shields.io/badge/PostgreSQL-15-blue)

A **full-stack blog platform** built with **ASP.NET Core Web API**, **React + TypeScript**, **Tailwind CSS**, and **PostgreSQL**. Users can register, login, create/edit posts, comment, like, and reset passwords.

---

## 🚀 Features

- User registration & login with **JWT authentication**  
- Forgot / Reset Password functionality  
- Create, read, update, delete blog posts  
- Like posts and comment system  
- Responsive UI with Tailwind CSS  
- Secure endpoints with role-based authorization  

---

## 📸 Screenshots

### **Login Page**
![Login](./docs/images/login.JPG)

### **Register Page**
![Register](./docs/images/register.JPG)

### **Forgot Password**
![Forgot Password](./docs/images/forgot-password.JPG)

### **Home Page**
![Home](./docs/images/home.JPG)

### **Create Post**
![Create Post](./docs/images/create-post.JPG)

### **Posts Listing**
![Post list](./docs/images/posts.JPG)
### **Individual Post / Comments**
![Post Details](./docs/images/post-details.JPG)

### **Logout Click**
![Logout](./docs/images/logout.JPG)


---

## 🛠️ Tech Stack

| Frontend      | Backend       | Database       | Others          |
|---------------|---------------|----------------|----------------|
| React + TS    | ASP.NET Core  | PostgreSQL     | Tailwind CSS    |
| React Router  | EF Core       |                | Axios           |
| React Context | JWT Auth      |                | Swagger         |

---

## ⚙️ Setup

1. Clone the repository:

```bash
git clone https://github.com/yourusername/BlogNest.git
```

2. Backend:

```bash
cd BlogNest/BlogNest
dotnet restore
dotnet run
```

3. Frontend:

```bash
cd BlogNest/BlogNestReactApp
npm install
npm run dev
```

4. Access in browser: `http://localhost:5173`

---

## 🔗 Links

- [Backend Documentation](./docs/Setup-dotnet.md)  
- [Frontend Documentation](./docs/Setup-reactapp.md)  
- [Project Plan & Workflow](./docs/PlanandWorkflow.md)
