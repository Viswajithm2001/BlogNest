# 📝 BlogNest – Backend Setup Documentation

This guide explains how to set up the BlogNest ASP.NET Core Web API project with PostgreSQL.

---

## 🚀 Project Tech Stack

- ASP.NET Core Web API
- PostgreSQL
- Entity Framework Core
- JWT Authentication
- Swagger (for API testing)

---

## 📁 Project Structure

BlogNest/
│
├── Controllers/
├── Models/
├── Data/
├── docs/
│ └── Setup.md
├── appsettings.json
├── Program.cs
└── ...

yaml
Copy
Edit

---

## ⚙️ Setup Instructions

### ✅ 1. Create PostgreSQL Database

- Install PostgreSQL from [https://www.postgresql.org/download/](https://www.postgresql.org/download/)
- During installation:
  - Set a superuser password (you'll need this later)
  - Leave the default port as `5432`
- Open **pgAdmin 4**
  - Right-click **Databases → Create → Database**
  - Name it: `BlogNestDb`

---

### ✅ 2. Create ASP.NET Core Web API Project

```bash
dotnet new webapi -n BlogNest
cd BlogNest
```
---
### ✅ 3. Install nuget packages
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
