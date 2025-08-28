-- ====================================================
-- File: OfficeAutomationDB.sql
-- Sample database for Office Automation (optimized)
-- ====================================================

-- ====================================================
-- Create Database
-- ====================================================
IF DB_ID('OfficeAutomationDB') IS NOT NULL
    DROP DATABASE OfficeAutomationDB;
GO

CREATE DATABASE OfficeAutomationDB;
GO

USE OfficeAutomationDB;
GO

-- ====================================================
-- Table: Users
-- ====================================================
CREATE TABLE dbo.Users(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(256) NULL,
    PasswordSalt NVARCHAR(36) NOT NULL,
    PasswordHash VARBINARY(32) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE UNIQUE NONCLUSTERED INDEX UQ_Users_Username ON dbo.Users(Username);
CREATE UNIQUE NONCLUSTERED INDEX UQ_Users_Email ON dbo.Users(Email);

-- ====================================================
-- Table: Roles
-- ====================================================
CREATE TABLE dbo.Roles(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);
CREATE UNIQUE NONCLUSTERED INDEX UQ_Roles_Name ON dbo.Roles(Name);

-- ====================================================
-- Table: UserRoles
-- ====================================================
CREATE TABLE dbo.UserRoles(
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY(UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY(RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);

-- ====================================================
-- Table: CartableTasks
-- ====================================================
CREATE TABLE dbo.CartableTasks(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CreatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'New',
    ReferenceNo NVARCHAR(50) NULL,
    CONSTRAINT FK_CartableTasks_Users FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
CREATE INDEX IX_CartableTasks_UserId ON dbo.CartableTasks(UserId);
CREATE INDEX IX_CartableTasks_Status ON dbo.CartableTasks(Status);
CREATE INDEX IX_CartableTasks_CreatedOn ON dbo.CartableTasks(CreatedOn);

-- ====================================================
-- Table: TaskFiles (optimized)
-- ====================================================
CREATE TABLE dbo.TaskFiles(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    UploadedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_TaskFiles_CartableTasks FOREIGN KEY(TaskId) REFERENCES CartableTasks(Id) ON DELETE CASCADE
);

-- ====================================================
-- Sample Data
-- ====================================================
-- Roles
INSERT INTO Roles(Name) VALUES ('Admin'), ('User');

-- Users
INSERT INTO Users(Username, Email, PasswordSalt, PasswordHash, IsActive)
VALUES 
('reza','reza@example.com','SALT-REZA-2025',0x66FC8389E376289DA1D290AC15847B0B84498F2A987CA942962F81511B7261E9,1),
('admin','admin@example.com','SALT-ADMIN-2025',0x77E6FEBDD31A6223D50C988A1D7FA1CC3ABA30A9C4AD73AE424C03C9353CD400,1),
('ali','ali@example.com','SALT-ALI-2025',0xD3D1043AC4C237E33C8006ABB2010F2E893AAF65AB53281E1D928AB4169B6832,1);

-- UserRoles
INSERT INTO UserRoles(UserId, RoleId) VALUES (1,2),(2,1),(3,2);

-- CartableTasks
INSERT INTO CartableTasks(UserId, Title, Description, Status, ReferenceNo)
VALUES
(1,'Document Review','Review project documents by 2025-09-10','New','REF-20250910-001'),
(1,'Approve Request','Approve the pending request by team','Read','REF-20250905-002'),
(3,'Prepare Report','Prepare financial report for August','New','REF-20250830-010');

-- TaskFiles sample
INSERT INTO TaskFiles(TaskId, FileName, FilePath)
VALUES (1,'ProjectPlan.pdf','C:\OfficeAutomation\Files\ProjectPlan.pdf');

-- ====================================================
-- Stored Procedures
-- ====================================================
-- Get tasks by user (optimized)
IF OBJECT_ID('dbo.sp_GetTasksByUser','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetTasksByUser;
GO
CREATE PROCEDURE dbo.sp_GetTasksByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Title, Status, ReferenceNo, CreatedOn
    FROM CartableTasks
    WHERE UserId = @UserId
    ORDER BY CreatedOn DESC;
END;
GO

-- Task report summary (optimized)
IF OBJECT_ID('dbo.sp_GetTaskReport','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetTaskReport;
GO
CREATE PROCEDURE dbo.sp_GetTaskReport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.Id AS UserId,
        u.Username,
        COUNT(CASE WHEN t.Status='New' THEN 1 END) AS NewTasks,
        COUNT(CASE WHEN t.Status='Read' THEN 1 END) AS ReadTasks,
        COUNT(CASE WHEN t.Status='Completed' THEN 1 END) AS CompletedTasks,
        MAX(t.CreatedOn) AS LastTaskDate
    FROM Users u
    LEFT JOIN CartableTasks t ON t.UserId = u.Id
    GROUP BY u.Id, u.Username
    ORDER BY u.Username;
END;
GO

-- ====================================================
-- Test Stored Procedures
-- ====================================================
-- Execute for specific user
-- EXEC dbo.sp_GetTasksByUser @UserId = 1;

-- Execute task report summary
-- EXEC dbo.sp_GetTaskReport;
