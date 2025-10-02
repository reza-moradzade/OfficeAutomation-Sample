# OfficeAutomation Sample Database (PostgreSQL)

This is a **sample PostgreSQL database** designed for a **small but professional Office Automation system**.  
The goal is to **demonstrate PostgreSQL database design, security, auditing, and optimization** in a compact structure, suitable for **portfolio and learning purposes**.

---

## Technologies

- **Database:** PostgreSQL  
- **ORM / API:** None (pure SQL / PL/pgSQL implementation)  
- **Tools:** pgAdmin, Git  
- **Purpose:** Portfolio, learning, and demonstration of professional PostgreSQL practices  

---

## Key Features

### 1 User Management & Security
- **Users, Roles, UserRoles:** Role-Based Access Control (RBAC)  
- **EmailVerifications:** Email confirmation workflow  
- **PasswordHash + PasswordSalt:** Secure authentication mechanism  
- **IsDeleted + xmin / version column:** Soft deletes and concurrency control using PostgreSQL system columns  

### 2 Sessions & Authentication
- **UserSessions:** Tracks active sessions and `last_activity` timestamp  
- **RefreshTokens:** Token lifecycle management (`is_revoked`, `replaced_by_token`)  
- **CaptchaAttempts:** Prevents automated login attempts  
- **FailedLoginAttempts:** Detects brute-force attacks  
- Supports **single active session per client type**  

### 3 Auditing & Logging
- **AuditLogs + AuditLogs_Archive:** Activity tracking with partitioning or separate archive table  
- Indexed for **fast reporting and monitoring**  
- Logs linked to `user_id` for accountability  
- Uses **triggers** for automatic audit logging where appropriate  

### 4 Task & File Management
- **Tasks + Cartable:** Workflow management and user inbox (Kartable)  
- **Files:** File attachments with metadata and versioning  
- Optimized for **fast retrieval** using indexes and foreign key relationships  

### 5 Optimization & Best Practices
- Proper **B-Tree and GIN indexes** on sessions, logs, and user tables  
- Uses **EXCLUDE constraints** and transactions for concurrency safety  
- Demonstrates **enterprise-grade PostgreSQL schema design** in small scale  
- Leverages **JSONB columns** for flexible metadata storage if needed  

### 6 Monitoring & Maintenance
- `last_activity` enables detection of inactive sessions  
- `xmin` / custom `version` column ensures safe updates under concurrency  
- Stored procedures / functions for:
  - **Archiving logs** (`fn_archive_audit_logs()`)  
  - **Cleaning up expired sessions** (`fn_cleanup_expired_sessions()`)  

---

## Getting Started

1. Run the provided PostgreSQL SQL script inside **pgAdmin**, **DBeaver**, or `psql`.  
2. Review database objects: **tables, views, indexes, functions, and triggers**.  
3. Test queries to explore **user sessions, auditing, and cartable functionality**.  

---

## Notes

- This database is a **sample** and **not intended for production use**.  
- Large files should be stored externally (e.g., filesystem, object storage).  
- Tokens and sessions are managed for **learning and demonstration purposes**.  
- Always use **`xmin` / version columns** in update operations to avoid conflicts.  
