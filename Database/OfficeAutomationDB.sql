-- ============================================================
-- Database: OfficeAutomationDB
-- Description: Sample Hybrid Application DB
-- Features: Secure Authentication, Authorization, Audit, 
--           Online Status, Concurrency Control, File Management
-- Suitable for: API + Blazor + WinForms sample project
-- ============================================================

-- =============================
-- Step 0: Drop database if exists
-- =============================
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'OfficeAutomationDB')
BEGIN
    -- Ensure no one is connected
    ALTER DATABASE [OfficeAutomationDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [OfficeAutomationDB];
END
GO

-- =============================
-- Step 1: Create database
-- =============================
CREATE DATABASE [OfficeAutomationDB];
GO

USE [OfficeAutomationDB];
GO

-- =============================
-- Step 2: Users Table
-- Description: Stores application users
-- Security: Password stored as hash + salt (bcrypt/Argon2 recommended)
-- =============================
CREATE TABLE [dbo].[Users](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL UNIQUE, -- Unique username
    [Email] NVARCHAR(256) NULL,
    [PasswordSalt] NVARCHAR(36) NOT NULL,      -- For hashing
    [PasswordHash] VARBINARY(64) NOT NULL,     -- Hash of password
    [IsActive] BIT NOT NULL DEFAULT 1,         -- Account active flag
    [CreatedOn] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- =============================
-- Step 3: Roles Table
-- Description: Stores roles for Role-Based Authorization
-- Example Roles: Admin, User
-- =============================
CREATE TABLE [dbo].[Roles](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- =============================
-- Step 4: UserRoles Table
-- Description: Mapping table between Users and Roles
-- Supports: Many-to-Many relationship
-- =============================
CREATE TABLE [dbo].[UserRoles](
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY(UserId, RoleId),
    FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY(RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);
GO

-- =============================
-- Step 5: CartableTasks Table
-- Description: Stores tasks assigned to users
-- Features:
--   - RowVersion: concurrency control for multi-user edits
--   - Status: New, Read, Completed
--   - Default values and constraints
-- =============================
CREATE TABLE [dbo].[CartableTasks](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,                       -- Owner of the task
    [Title] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [CreatedOn] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'New',
    [ReferenceNo] NVARCHAR(50) NULL,
    [RowVersion] ROWVERSION,                     -- For optimistic concurrency
    FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- Index to improve query performance for large number of users/tasks
CREATE NONCLUSTERED INDEX IX_CartableTasks_UserId_Status
ON CartableTasks(UserId, Status);
GO

-- =============================
-- Step 6: TaskFiles Table
-- Description: Stores file attachments for tasks
-- Notes:
--   - FileUrl: points to storage location (local or cloud)
--   - UploadedOn: default to current timestamp
-- =============================
CREATE TABLE [dbo].[TaskFiles](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [TaskId] INT NOT NULL,
    [FileName] NVARCHAR(255) NOT NULL,
    [FileUrl] NVARCHAR(500) NOT NULL,
    [UploadedOn] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY(TaskId) REFERENCES CartableTasks(Id) ON DELETE CASCADE
);
GO

-- =============================
-- Step 7: UserSessions Table
-- Description: Tracks online status for Hybrid App
-- Fields:
--   - IsOnlineBlazor: User logged in via Blazor
--   - IsOnlineWinForms: User logged in via WinForms
--   - LastActivity: timestamp for Heartbeat / Timeout
-- Notes: Prevents same user using both clients simultaneously
-- =============================
CREATE TABLE [dbo].[UserSessions](
    [UserId] INT NOT NULL PRIMARY KEY,
    [IsOnlineBlazor] BIT NOT NULL DEFAULT 0,
    [IsOnlineWinForms] BIT NOT NULL DEFAULT 0,
    [LastActivity] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- =============================
-- Step 8: AuditLogs Table
-- Description: Records all critical actions for security & traceability
-- Fields: ActionType, Description, ActionDate
-- =============================
CREATE TABLE [dbo].[AuditLogs](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [ActionType] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [ActionDate] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- =============================
-- Step 9: FailedLoginAttempts Table
-- Description: Tracks failed login attempts to prevent brute force
-- =============================
CREATE TABLE [dbo].[FailedLoginAttempts](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [AttemptDate] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
    [IPAddress] NVARCHAR(50) NULL,
    FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

-- =============================
-- Step 10: Stored Procedures
-- Features:
--   - Safe parameterized queries to prevent SQL Injection
--   - Example: sp_UserLogin, sp_GetTaskReport
-- =============================

-- Login Procedure: verifies user credentials safely
CREATE PROCEDURE [dbo].[sp_UserLogin]
    @Username NVARCHAR(100),
    @PasswordHash VARBINARY(64)
AS
BEGIN
    SET NOCOUNT ON;

    -- Safe parameterized check
    SELECT u.Id, u.Username, u.IsActive
    FROM Users u
    WHERE u.Username = @Username
      AND u.PasswordHash = @PasswordHash
      AND u.IsActive = 1;
END;
GO

-- Task Report Procedure
CREATE PROCEDURE [dbo].[sp_GetTaskReport]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.Id AS UserId,
        u.Username,
        COUNT(CASE WHEN t.Status = 'New' THEN 1 END) AS NewTasks,
        COUNT(CASE WHEN t.Status = 'Read' THEN 1 END) AS ReadTasks,
        COUNT(CASE WHEN t.Status = 'Completed' THEN 1 END) AS CompletedTasks,
        MAX(t.CreatedOn) AS LastTaskDate
    FROM Users u
    LEFT JOIN CartableTasks t ON t.UserId = u.Id
    GROUP BY u.Id, u.Username
    ORDER BY u.Username;
END;
GO

-- =============================
-- Step 11: Sample Data
-- =============================

-- Roles
INSERT INTO Roles(Name) VALUES ('Admin'), ('User');

-- Users
INSERT INTO Users(Username, Email, PasswordSalt, PasswordHash)
VALUES 
('reza', 'reza@example.com', 'SALT-REZA-2025', 0x66FC8389E376289DA1D290AC15847B0B84498F2A987CA942962F81511B7261E9),
('admin', 'admin@example.com', 'SALT-ADMIN-2025', 0x77E6FEBDD31A6223D50C988A1D7FA1CC3ABA30A9C4AD73AE424C03C9353CD400),
('ali', 'ali@example.com', 'SALT-ALI-2025', 0xD3D1043AC4C237E33C8006ABB2010F2E893AAF65AB53281E1D928AB4169B6832);

-- UserRoles
INSERT INTO UserRoles(UserId, RoleId) VALUES
(1,2), -- reza -> User
(2,1), -- admin -> Admin
(3,2); -- ali -> User

-- Sample Tasks
INSERT INTO CartableTasks(UserId, Title, Description, Status, ReferenceNo)
VALUES
(1, 'Document Review', 'Review project documents by 2025-09-10', 'New', 'REF-20250910-001'),
(1, 'Approve Request', 'Approve the pending request by team', 'Read', 'REF-20250905-002'),
(3, 'Prepare Report', 'Prepare financial report for August', 'New', 'REF-20250830-010');

-- Sample TaskFiles
INSERT INTO TaskFiles(TaskId, FileName, FileUrl)
VALUES
(1, 'ProjectPlan.pdf', 'https://storage.example.com/ProjectPlan.pdf');
GO

-- ============================================================
-- ✅ Notes for GitHub / Presentation:
-- 1. Secure authentication with hash+salt
-- 2. Role-Based Authorization + UserRoles
-- 3. Task management with concurrency control (RowVersion)
-- 4. Online status tracking for hybrid apps (Blazor + WinForms)
-- 5. Audit logs + failed login attempts for security
-- 6. Parameterized stored procedures prevent SQL Injection
-- 7. Indexing for performance with large user base
-- 
