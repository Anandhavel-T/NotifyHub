
# NotifyHub

## 📌 Project Overview
NotifyHub is a notification management system designed for administrators to send, schedule, and track notifications for customers. The application is built using **.NET Framework 4.8**, **Entity Framework (MySQL)**, and follows the **repository pattern** for scalable architecture.

The application allows administrators to:  
✔ **Manage Users** – Create, edit, and delete admin users  
✔ **Send Notifications** – Broadcast messages to all customers  
✔ **Schedule Notifications** – Set expiry dates for notifications  
✔ **Track Notification Status** – Mark notifications as read when viewed  
✔ **Future Enhancement** – Send targeted notifications to specific customers 

## 🚀 Features
- **Admin Authentication** – Secure login for administrators
- **Notification Management** – Create, schedule, and delete notifications
- **Customer Details Storage** – Maintain customer records
- **Notification Status Tracking** – Mark notifications as read when viewed
- **Scalability & Maintainability** – Uses repository pattern and clean architecture

## 🛠️ Tech Stack
- **Frontend:** HTML and Bootstrap 5
- **Backend:** .NET Web API with MVC
- **Database:** MySQL with Entity Framework
- **Architecture:** Clean Architecture + Repository Pattern
- **Hosting:** TBD

## ⚙️ Project Setup

### 1️⃣ **Clone the Repository**
```sh
git clone https://github.com/Anandhavel-T/NotifyHub.git
cd NotifyHub
```

### 2️⃣ **Database Setup (MySQL)**
Ensure MySQL is installed and running, then create a database:

```sql
CREATE DATABASE NotifyHubDB;
```

### 3️⃣ **Update Connection String**
Modify `appsettings.json` or `web.config` to configure your MySQL connection:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=NotifyHubDB;User Id=your_user;Password=your_password;"
}
```

### 4️⃣ **Run Database Migrations**
```sh
cd NotifyHub\Data\
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5️⃣ **Run the Application**
```sh
dotnet run
```
The application should now be running at `http://localhost:5000/`.

## 🔧 Development Guidelines
- Follow **Clean Architecture & Repository Pattern** for maintainability.
- Ensure **migrations are maintained in a single file** under `Migrations/`.
- Keep **controller actions minimal** and delegate business logic to **services**.

## 📌 Future Enhancements
- Sending notifications to specific customers
- Real-time notifications using SignalR
- Multi-container architecture deployment (Docker)


---

🚀 **Happy Coding!** 🎯

