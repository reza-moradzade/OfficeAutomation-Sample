# OfficeAutomation Sample Database

This is a **sample SQL Server database** designed for a **small but professional Office Automation system**.  
The main goal is to **demonstrate database structure design, security, auditing, and optimization** in a compact sample, ready for portfolio and learning purposes.

---

## 🔧 Technologies

- **Database:** SQL Server  
- **ORM / API:** None, implemented directly in SQL  
- **Tools:** SQL Server Management Studio, Visual Studio, Git  
- **Purpose:** Portfolio, learning, and demonstration of professional database design  

---

## 📂 Key Features

### 1️⃣ User Management & Security
- **Users, Roles, UserRoles:** Role-Based Access Control (RBAC)  
- **EmailVerifications:** Tracks email confirmation status  
- **PasswordHash** + **PasswordSalt** for secure authentication  
- **IsDeleted** + **RowVersion** columns for soft deletes and concurrency control  

### 2️⃣ Sessions & Authentication
- **UserSessions:** Tracks user sessions and last activity (`LastActivity`)  
- **RefreshTokens:** Full token lifecycle management (`IsRevoked`, `ReplacedByToken`)  
- **CaptchaAttempts:** Prevents automated login attempts  
- **Single active session per client type** enforced  

### 3️⃣ Auditing & Logging
- **AuditLogs & AuditLogs_Archive:** Complete activity tracking with archival support  
- **FailedLoginAttempts:** Monitors unsuccessful login attempts  
- Indexed for **fast reporting and monitoring**  

### 4️⃣ Task & File Management
- **Tasks + Cartable:** Manages tasks and user inbox (Kartable)  
- **Files:** Supports attachments linked to tasks  
- Optimized for **fast access to tasks and inbox items**  

### 5️⃣ Optimization & Sample Design
- Indexed for **quick lookup of users, sessions, logs, and tasks**  
- Designed for **concurrency and security testing** in a small sample  
- Demonstrates **professional database design best practices**  

### 6️⃣ Monitoring & Maintenance
- `LastActivity` column allows detecting inactive sessions  
- `RowVersion` ensures safe concurrent updates  
- Stored procedures provided for **archiving logs** and **cleaning up expired sessions**  

---

## 🛠️ Getting Started

1. Execute the provided SQL script (`OfficeAutomationDB.sql`) in **SQL Server Management Studio**  
2. Explore tables, indexes, and stored procedures to understand professional database structure  
3. Test queries and sample operations to experience session management, auditing, and task handling  

---

## 📌 Notes

- This is a **small sample database** and **not intended for production use**.  
- Large files should be stored externally (filesystem or object storage).  
- Tokens and sessions are **managed for demonstration and learning purposes**.  
- Always use `RowVersion` in updates to prevent data conflicts.
