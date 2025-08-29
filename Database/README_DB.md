# Office Automation Database

This repository contains the SQL Server database schema for an **Enterprise-Grade Office Automation System**.  
The database is designed to handle **20,000+ registered users** and up to **10,000 concurrent active sessions**, with a strong focus on **security, scalability, and performance**.

---

## 📂 Database Features

### 1. User Management
- **Users Table** with `PasswordHash + Salt` for secure authentication.
- **PasswordHistory** table to prevent password reuse.
- **UserRoles** and `Roles` for Role-Based Access Control (RBAC).
- `RowVersion` for concurrency control.
- `IsDeleted` column for **soft deletes** (data never physically deleted).

### 2. Authentication & Sessions
- **UserSessions** with `SessionToken`, `ClientType` (Web / Windows / Mobile).
- `LastActivity` column to detect inactivity.
- Enforced **single active session** (cannot open both Web and Windows client simultaneously).
- `RowVersion` for concurrency safety.

### 3. Audit & Logging
- **AuditLogs** table stores every user activity.
- Optimized for large volumes with indexes and **partitioning strategy**.
- **Archiving** supported: old logs can be moved to `AuditLogs_Archive`.

### 4. Task Management
- **Tasks Table** with `Status`, `AssignedTo`, `DueDate`.
- `RowVersion` and `IsDeleted` for safe concurrency and soft deletion.

### 5. File Management
- **Files Table** supports document storage:
  - `FileName`, `ContentType`, `FileSize`
  - `FileContent VARBINARY(MAX)` (for smaller files)
  - Recommendation: store large files in **File System or Object Storage (S3, MinIO, Azure Blob)** instead of database.
- `IsDeleted` for safe deletion.

---

## 🔑 Security Considerations
- Passwords stored as **hash + salt** (e.g., SHA-256 or PBKDF2).
- Sessions protected with **GUID tokens** and concurrency validation.
- Sensitive tables (`Users`, `Sessions`) protected with `RowVersion` to avoid race conditions.
- **Audit trail** ensures full accountability.

---

## ⚡ Performance & Scalability
- **Indexes**:
  - `IX_Users_UserName` for quick login lookups.
  - `IX_UserSessions_LastActivity` for session monitoring.
  - `IX_AuditLogs_Timestamp` for fast reporting.
- **Partitioning**: AuditLogs can be partitioned by date.
- **Archiving**: AuditLogs older than X months should be moved to archive table.
- Designed for **10K concurrent sessions** without deadlocks.

---

## 📊 Monitoring & Maintenance
- **SystemStats** table can be added to track:
  - Peak concurrent users
  - Average session duration
  - Failed logins
- Regular cleanup job for `UserSessions` to remove expired sessions.
- Archive old `AuditLogs` periodically.

---

## 🛠️ Deployment
1. Run `DatabaseSchema.sql` in SQL Server.
2. Configure your application (Web, Windows, or Mobile client) to use the connection string.
3. Apply scheduled jobs for:
   - Audit log archiving
   - Session cleanup
   - Backup strategy

---

## 📌 Best Practices
- Use **Redis** for session caching if scaling to multiple servers.
- Store **large files outside the DB** (filesystem or object storage).
- Enable **Transparent Data Encryption (TDE)** for sensitive environments.
- Always run DB under **least-privilege service account**.
- Implement **row-level security** if different departments use the same DB.

---

## 📂 Repository Structure
