# **BlogNest Frontend Setup Guide**

This document covers the initial setup for the **BlogNest React frontend** using **Vite + TypeScript + Tailwind CSS v4**.

---

## **1. Create Vite Project**
```bash
npm create vite@latest
```
- **Why?**  
  Vite is a fast build tool for modern frontend development. We use it to scaffold our React + TypeScript app quickly.

---

## **2. Install Dependencies**
```bash
npm install
```
- **Why?**  
  Installs all required packages listed in `package.json` (React, Vite, TypeScript, etc.) so the project can run.

---

## **3. Run Development Server**
```bash
npm run dev
```
- **Why?**  
  Starts a local dev server with **hot reloading** — changes to source files update instantly in the browser.

---

## **4. Tailwind CSS v4 Setup**
> Tailwind v4 is zero-config by default.

1. Install Tailwind CSS, PostCSS, and Autoprefixer (already in `devDependencies` if you used this template).
2. In `src/index.css`, add:
```css
@import "tailwindcss";
```
- **Why?**  
  Tailwind provides a utility-first CSS framework for rapid UI development.  
  `@import "tailwindcss";` enables Tailwind styles globally.

---

## **5. Install React Router**
```bash
npm install react-router-dom
```
- **Why?**  
  Enables client-side routing for pages like `/`, `/login`, `/register`, and `/posts`.

---

## **6. Install Axios**
```bash
npm install axios
```
- **Why?**  
  Axios is used to make HTTP requests to our backend API, including attaching JWT tokens for authentication.

---

## **7. Project Folder Structure**
```
src/
  ├── components/   → Reusable UI parts (Navbar, Button, FormInput, etc.)
  ├── pages/        → Each page’s UI (Home, Login, BlogDetails, etc.)
  ├── context/      → Global state using React Context API
  ├── services/     → API calls using Axios
  ├── styles/       → Global styles (if needed)
  ├── App.tsx       → Main app component
  ├── index.css     → Tailwind base styles
  └── main.tsx      → Entry point of React app
```

---

## **8. Optional: Prettier for Code Formatting**
```bash
npm install -D prettier eslint-config-prettier
```
- **Why?**  
  Ensures consistent code style across the project by integrating with ESLint.

---

✅ **At this point, your BlogNest frontend environment is ready for development.**  
Next step: Implement routing and authentication in **Phase 1 – The React Awakens**.
