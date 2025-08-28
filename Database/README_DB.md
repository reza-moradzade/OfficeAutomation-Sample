# Office Automation Sample Database

This folder contains the SQL script `OfficeAutomationDB.sql` to create a **sample Office Automation database** for demonstration, testing, and educational purposes.

The database includes optimized examples of user and task management, file storage, and reporting via stored procedures. It demonstrates best practices in database design and optimization.

---

## Database Structure

| Table | Description |
|-------|-------------|
| `Users` | Stores user information: username, email, password hash, salt, active status, and creation timestamp |
| `Roles` | Stores system roles (Admin, User) |
| `UserRoles` | Many-to-many relationship linking users and roles |
| `CartableTasks` | Stores tasks assigned to users, including title, description, status, reference number, and created timestamp |
| `TaskFiles` | Optimized file storage: stores file metadata and disk paths instead of large binaries |

---

## Features

- Clean and consistent naming conventions  
- Proper data types and default values  
- Foreign key relationships with `ON DELETE CASCADE`  
- Indexes on `UserId`, `Status`, and `CreatedOn` for faster queries  
- Sample data for immediate testing  
- **Stored Procedures (`sp_GetTasksByUser`, `sp_GetTaskReport`) are optimized** to avoid heavy loops or inefficient code, ensuring fast execution even with large datasets  
- **File storage is optimized**: instead of storing large binary files in the database, only file metadata and disk paths are stored, reducing database size and improving performance  
- Ready to use with ASP.NET Core API, Blazor, or Windows Forms projects  

---

## Quick Start

1. Open **SQL Server Management Studio (SSMS)** or your preferred SQL Server client.  
2. Open and run `OfficeAutomationDB.sql` located in this folder.

   This script will:  
   - Create the `OfficeAutomationDB` database  
   - Create all tables with relationships and indexes  
   - Insert sample users, roles, tasks, and files  
   - Create sample stored procedures for optimized reporting  

3. Connect your application to the database:  
   - Server: your SQL Server instance  
   - Database: `OfficeAutomationDB`  
   - Authentication: SQL Server or Windows Authentication  

4. Test the database with a sample query:  
```sql
-- Get all tasks assigned to user 'reza'
SELECT t.* 
FROM CartableTasks t
JOIN Users u ON t.UserId = u.Id
WHERE u.Username = 'reza';
