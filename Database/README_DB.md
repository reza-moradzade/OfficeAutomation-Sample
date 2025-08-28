# Office Automation Sample Database

This folder contains the **SQL script** to create a sample Office Automation database for demonstration and testing purposes.

The database is designed for **educational, testing, and sample project purposes**. It includes users, roles, user-role assignments, and tasks (cartable items).

---

## Structure

| Table | Description |
|-------|-------------|
| `Users` | Stores user information: username, email, password hash, salt, active status, creation timestamp |
| `Roles` | Stores roles in the system (Admin, User) |
| `UserRoles` | Many-to-many relationship linking users and roles |
| `CartableTasks` | Stores tasks assigned to users, including title, description, status, reference number, and created timestamp |

---

## Features

- Clean and clear table and column naming
- Proper data types and default values
- Foreign key relationships with `ON DELETE CASCADE`
- Indexes on `UserId` and `Status` for faster queries
- Sample data for immediate testing
- Ready to use with ASP.NET Core API, Blazor, or Windows Forms projects

---

## Quick Start

1. Open **SQL Server Management Studio (SSMS)** or your preferred SQL Server client.
2. Open and run `OfficeAutomationDB.sql` located in this folder.
3. This script will:
   - Create the `OfficeAutomationDB` database
   - Create all tables with relationships
   - Insert sample users, roles, and tasks
4. Connect your application to the database:
   - Server: your SQL Server instance
   - Database: `OfficeAutomationDB`
   - Authentication: SQL Server or Windows Auth
5. Test the database with a sample query:

```sql
-- Get all tasks assigned to user 'reza'
SELECT t.* 
FROM CartableTasks t
JOIN Users u ON t.UserId = u.Id
WHERE u.Username = 'reza';
