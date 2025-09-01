Office Automation Database

This repository contains the SQL Server database schema for a scalable, secure, hybrid Office Automation System.
Designed to support 20,000+ registered users and 10,000+ concurrent sessions, with a strong focus on security, auditability, and performance.

📂 Database Features
1. User Management

Users Table with PasswordHash + PasswordSalt for secure authentication.

UserRoles and Roles tables implement Role-Based Access Control (RBAC).

RowVersion column enables optimistic concurrency control.

IsDeleted column supports soft deletes (records are never physically removed).

IsEmailConfirmed column tracks email verification status.

EmailVerifications table stores verification tokens and expiration timestamps for account confirmation.

2. Authentication & Sessions

UserSessions table tracks user sessions:

SessionToken uniquely identifies a session

ClientType (Web / Windows / Mobile)

LastActivity timestamp enables automatic session expiry detection

Enforced single active session per client type

RefreshTokens table supports token-based authentication with:

IsRevoked, RevokedAt, ReplacedByToken for full lifecycle management

RowVersion for concurrency safety

CaptchaAttempts table prevents automated login attempts and bots.

3. Audit & Logging

AuditLogs table stores all user activity with timestamp and IP.

FailedLoginAttempts table monitors and mitigates brute-force attacks.

AuditLogs_Archive table stores old logs for long-term retention.

Indexes optimize queries for reporting, monitoring, and auditing.

4. Task Management

Tasks Table supports:

Title, Description, AssignedTo, Status, DueDate, RowVersion, IsDeleted

Cartable Table stores user-specific task assignments (inbox):

Tracks IsRead and ReceivedAt timestamp

Enables safe multi-user task handling without conflicts.

Files Table linked to tasks supports attachments:

FileName, ContentType, FileSize, optional FileContent

Recommendation: store large files externally (S3, Azure Blob, MinIO)

5. Security Enhancements

Passwords are securely hashed and salted (recommend using bcrypt, PBKDF2, or Argon2).

Session tokens use GUIDs validated via RowVersion.

CaptchaAttempts table protects login/signup processes.

Email verification workflow ensures account confirmation.

All API interactions should use parameterized queries to prevent SQL injection.

Complete audit trail ensures full accountability for all user actions.

6. Performance & Scalability

Indexed for fast operations:

IX_Users_UserName_Active — quickly find active users

IX_UserSessions_LastActivity — detect idle sessions efficiently

IX_AuditLogs_Timestamp — support audit reporting

IX_Tasks_AssignedTo_Status — fast task lookups

IX_Cartable_UserId_IsRead_ReceivedAt — optimize inbox queries

Designed for 10,000+ concurrent sessions.

Supports horizontal scaling with caching (Redis) and external file storage.

7. Monitoring & Maintenance

LastActivity in UserSessions table allows idle session detection.

Track system statistics (peak concurrent users, average session duration, failed logins) using optional monitoring tables.

Audit logs can be archived to reduce table size and maintain performance.

RowVersion ensures safe concurrent updates.

🛠️ Deployment Steps

Execute OfficeAutomationDB_Final.sql in SQL Server Management Studio.

Configure connection strings in Web, WinForms, or Mobile applications.

Follow security best practices:

Use least-privilege accounts

Enforce TLS/HTTPS for application access

Consider row-level security for multi-department deployments (optional)

📌 Best Practices

Store large files outside the database (filesystem or object storage).

Enable Transparent Data Encryption (TDE) for sensitive environments.

Use Redis or similar caching for high concurrency session management.

Monitor FailedLoginAttempts, AuditLogs, and CaptchaAttempts for security incidents.

Always use RowVersion in update operations to prevent concurrency conflicts.

Validate EmailVerifications tokens and handle expiration correctly.

This database is ready for hybrid deployment (Blazor Web + WinForms + API backend) and optimized for high concurrency while maintaining strong security and audit compliance.
