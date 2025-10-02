# OfficeAutomation Sample Database

This is a **sample SQL Server database** designed for a **small but professional Office Automation system**.  
The goal is to **demonstrate database design, security, auditing, and optimization** in a compact structure, suitable for **portfolio and learning purposes**.

---

## echnologies

- **Database:** SQL Server  
- **ORM / API:** None (pure SQL implementation)  
- **Tools:** SQL Server Management Studio, Visual Studio, Git  
- **Purpose:** Portfolio, learning, and demonstration of professional database practices  

---

## Key Features

### 1 User Management & Security
- **Users, Roles, UserRoles:** Role-Based Access Control (RBAC)  
- **EmailVerifications:** Email confirmation workflow  
- **PasswordHash + PasswordSalt:** Secure authentication mechanism  
- **IsDeleted + RowVersion:** Soft deletes and concurrency control  

### 2 Sessions & Authentication
- **UserSessions:** Tracks active sessions and `LastActivity`  
- **RefreshTokens:** Token lifecycle management (`IsRevoked`, `ReplacedByToken`)  
- **CaptchaAttempts:** Prevents automated login attempts  
- **FailedLoginAttempts:** Detects brute-force attacks  
- Supports **single active session per client type**  

### 3 Auditing & Logging
- **AuditLogs + AuditLogs_Archive:** Activity tracking with archival  
- Indexed for **fast reporting and monitoring**  
- Logs linked to `UserId` for accountability  

### 4 Task & File Management
- **Tasks + Cartable:** Workflow management and user inbox (Kartable)  
- **Files:** File attachments with metadata and versioning  
- Optimized for **fast retrieval of tasks and files**  

### 5 Optimization & Best Practices
- Proper **indexes** on sessions, logs, and user tables  
- Designed for **security, concurrency, and performance testing**  
- Demonstrates **enterprise-grade schema design in small scale**  

### 6 Monitoring & Maintenance
- `LastActivity` enables detection of inactive sessions  
- `RowVersion` ensures safe updates under concurrency  
- Stored procedures for:
  - **Archiving logs** (`sp_ArchiveAuditLogs`)  
  - **Cleaning up expired sessions** (`sp_CleanupExpiredSessions`)  

---

## Getting Started

1. Run the provided SQL script inside **SQL Server Management Studio**.  
2. Review database objects: **tables, views, indexes, and stored procedures**.  
3. Test queries to explore **user sessions, auditing, and cartable functionality**.  

---

## Notes

- This database is a **sample** and **not intended for production use**.  
- Large files should be stored externally (e.g., filesystem, object storage).  
- Tokens and sessions are managed for **learning and demonstration purposes**.  
- Always use **RowVersion** in update operations to avoid conflicts.  
