# Office Automation Database (Final Version)

This repository contains the **SQL Server database schema** for a **scalable, secure, hybrid Office Automation System**.  
Designed to support **20,000+ registered users** and **10,000+ concurrent sessions**, with a strong focus on **security, auditability, and performance**.

---

## 📂 Database Features

### 1. User Management
- **Users Table** with `PasswordHash + Salt` for secure authentication.
- **UserRoles** and `Roles` for Role-Based Access Control (RBAC).
- `RowVersion` column for **optimistic concurrency**.
- `IsDeleted` column for **soft deletes** (records are never physically deleted).

### 2. Authentication & Sessions
- **UserSessions** table tracks sessions with:
  - `SessionToken`, `ClientType` (Web / Windows / Mobile)
  - `LastActivity` timestamp for inactivity detection
  - Enforced **single active session per client type** (cannot use Web and WinForms simultaneously)
- `RowVersion` ensures concurrency safety.
- Stored Procedures:
  - `sp_UserLogin` – login verification
  - `sp_UserLogout` – logout handling
  - `sp_KillSession` – admin forced logout
  - `sp_CleanupExpiredSessions` – automatic session cleanup

### 3. Audit & Logging
- **AuditLogs** table stores all user activity.
- **FailedLoginAttempts** tracks and prevents brute-force attacks.
- **Archiving**:
  - `sp_ArchiveAuditLogs` moves old logs to `AuditLogs_Archive`.
- Indexed for reporting and monitoring.

### 4. Task Management
- **Tasks Table** supports:
  - `Status`, `AssignedTo`, `DueDate`, `RowVersion`, `IsDeleted`
- Enables safe multi-user task handling without conflicts.
- **Files Table** linked to tasks supports attachments:
  - `FileName`, `ContentType`, `FileSize`, optional `FileContent`
  - Recommendation: store large files externally (S3, Azure Blob, MinIO)

### 5. Security Considerations
- Passwords hashed and salted (recommend bcrypt, PBKDF2, or Argon2).
- Session tokens use GUIDs and validated via `RowVersion`.
- Sensitive tables (`Users`, `UserSessions`) are protected for **race conditions**.
- Audit trail ensures full accountability.
- Only parameterized queries allowed for all SPs to prevent SQL Injection.

### 6. Performance & Scalability
- Indexed for fast login, session monitoring, and audit reporting:
  - `IX_Users_UserName`
  - `IX_UserSessions_LastActivity`
  - `IX_AuditLogs_Timestamp`
- Designed to handle **10,000+ concurrent sessions**.
- Use caching (Redis) and external storage for large files when scaling horizontally.
- Scheduled jobs recommended:
  - Session cleanup
  - Audit log archiving
  - Database backups

### 7. Monitoring & Maintenance
- `LastActivity` in `UserSessions` can detect idle sessions.
- Cleanup expired sessions periodically with `sp_CleanupExpiredSessions`.
- Track system stats (peak concurrent users, average session duration, failed logins) with optional `SystemStats` table.
- Archive old audit logs to reduce table size.

---

## 🛠️ Deployment Steps
1. Execute `OfficeAutomationDB_Final.sql` in **SQL Server Management Studio**.
2. Configure connection string in **Web, WinForms, or Mobile application**.
3. Schedule jobs for:
   - Cleanup expired sessions
   - Archive audit logs
   - Backups
4. Implement security best practices:
   - Least-privilege accounts
   - TLS/HTTPS for application access
   - Row-level security for multi-department environments (optional)

---

## 📌 Best Practices
- Store large files outside the DB (filesystem or object storage).  
- Enable **Transparent Data Encryption (TDE)** for sensitive environments.  
- Use **Redis** or similar caching for high concurrency session management.  
- Regularly monitor `FailedLoginAttempts` and `AuditLogs` for security incidents.  
- Ensure `RowVersion` is always used in update operations to prevent concurrency conflicts.  

---

## 📂 Repository Structure
