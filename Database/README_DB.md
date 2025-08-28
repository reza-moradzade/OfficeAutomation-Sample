# OfficeAutomationDB - Sample Hybrid Application Database

## Overview
This repository contains a **sample database schema** for a hybrid application consisting of **Blazor Web, WinForms Desktop, and API backend**. It demonstrates **best practices in authentication, authorization, audit, and security** while being scalable for a large number of users (~22,000+).

The database is designed for educational, demonstration, and prototype purposes.

---

## Features

### 1. Secure Authentication
- Passwords are stored using **hash + salt** (bcrypt/Argon2 recommended)
- Users table includes `Username`, `Email`, `PasswordSalt`, `PasswordHash`
- Active flag (`IsActive`) to disable accounts

### 2. Role-Based Authorization
- `Roles` table with standard roles (Admin, User)
- `UserRoles` mapping table for many-to-many relationship
- Supports RBAC in API / Blazor / WinForms

### 3. Task Management
- `CartableTasks` table for tasks assigned to users
- `TaskFiles` table stores task attachments (URL-based storage)
- `RowVersion` used for **optimistic concurrency** (prevents conflicts)

### 4. Hybrid App Online Tracking
- `UserSessions` table tracks online status per client
  - `IsOnlineBlazor` for web
  - `IsOnlineWinForms` for desktop
- Prevents a user from being logged in on multiple clients simultaneously

### 5. Audit and Logging
- `AuditLogs` for recording all critical actions
- `FailedLoginAttempts` for brute force protection

### 6. Security Best Practices
- Stored Procedures are **parameterized** to prevent SQL Injection
- Users have **limited database privileges** (recommended)
- Default values and constraints to maintain data integrity

### 7. Performance
- Indexed `CartableTasks(UserId, Status)` for fast queries
- Efficient schema for large user base

---

## Tables

| Table | Description |
|-------|-------------|
| `Users` | Stores application users and credentials |
| `Roles` | Stores user roles for RBAC |
| `UserRoles` | Maps users to roles |
| `CartableTasks` | Task assignments with status |
| `TaskFiles` | Attachments for tasks |
| `UserSessions` | Online status tracking for hybrid app |
| `AuditLogs` | Logs critical actions |
| `FailedLoginAttempts` | Tracks failed login attempts |

---

## Stored Procedures

| SP Name | Description |
|---------|-------------|
| `sp_UserLogin` | Parameterized login procedure (prevents SQL Injection) |
| `sp_GetTaskReport` | Returns task summary per user |

---

## Sample Data

- Users: `reza`, `admin`, `ali`
- Roles: `Admin`, `User`
- Tasks: 3 sample tasks
- TaskFiles: 1 sample file attachment

---

## Getting Started

1. Open **SQL Server Management Studio** (SSMS)
2. Execute the script `OfficeAutomationDB.sql` to create the database and sample data
3. Use the database in your **API / Blazor / WinForms** application
4. Modify roles, tasks, and users as needed for your application

---

## Security Considerations

- **Always use parameterized queries** in your application
- Limit direct access to the database (`db_owner` is not recommended)
- Enable **HTTPS** for API and web applications
- Implement **MFA / 2FA** in application layer
- Monitor `AuditLogs` and `FailedLoginAttempts` regularly

---

## Notes for Developers

- Optimized for hybrid demonstration (WinForms + Blazor + API)
- Concurrency handled via `RowVersion`
- Designed to scale for large user base
- Fully commented schema for learning and presentation

---

## License

This is a sample educational project. You are free to use and modify it for **learning, prototyping, and demonstration purposes**.
