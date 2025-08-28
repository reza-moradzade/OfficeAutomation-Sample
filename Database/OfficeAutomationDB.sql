-- ====================================================
-- Database: OfficeAutomationDB
-- ====================================================
-- This script creates a sample Office Automation database
-- including users, roles, user-role assignments, and tasks (cartable items)
-- It is intended for testing and demonstration purposes.
-- ====================================================

-- Drop the database if it exists (for clean setup)
IF DB_ID('OfficeAutomationDB') IS NOT NULL
    DROP DATABASE OfficeAutomationDB;
GO

-- Create the database
CREATE DATABASE OfficeAutomationDB;
GO

-- Use the new database
USE OfficeAutomationDB;
GO

-- ====================================================
-- Table: Users
-- ====================================================
-- This table stores application users
CREATE TABLE [dbo].[Users](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,          -- Primary key
    [Username] NVARCHAR(100) NOT NULL,           -- Unique username
    [Email] NVARCHAR(256) NULL,                  -- User email (optional)
    [PasswordSalt] NVARCHAR(36) NOT NULL,        -- Salt for password hashing
    [PasswordHash] VARBINARY(32) NOT NULL,       -- Hashed password
    [IsActive] BIT NOT NULL DEFAULT 1,           -- Status flag: 1=active, 0=inactive
    [CreatedOn] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME() -- Record creation time
);

-- Unique constraints
CREATE UNIQUE NONCLUSTERED INDEX UQ_Users_Username ON [dbo].[Users]([Username]);
CREATE UNIQUE NONCLUSTERED INDEX UQ_Users_Email ON [dbo].[Users]([Email]);

-- ====================================================
-- Table: Roles
-- ====================================================
-- This table stores application roles (Admin, User, etc.)
CREATE TABLE [dbo].[Roles](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,          -- Primary key
    [Name] NVARCHAR(50) NOT NULL                 -- Role name
);

-- Ensure role names are unique
CREATE UNIQUE NONCLUSTERED INDEX UQ_Roles_Name ON [dbo].[Roles]([Name]);

-- ====================================================
-- Table: UserRoles
-- ====================================================
-- Many-to-many relationship table linking users and roles
CREATE TABLE [dbo].[UserRoles](
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY(RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);

-- ====================================================
-- Table: CartableTasks
-- ====================================================
-- This table stores tasks assigned to users (previously CartableItems)
CREATE TABLE [dbo].[CartableTasks](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,          -- Primary key
    [UserId] INT NOT NULL,                        -- Foreign key to Users table
    [Title] NVARCHAR(200) NOT NULL,              -- Task title
    [Description] NVARCHAR(MAX) NULL,            -- Task details
    [CreatedOn] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), -- Task creation time
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'New', -- Task status (New, Read, Completed)
    [ReferenceNo] NVARCHAR(50) NULL,            -- Optional reference number
    CONSTRAINT FK_CartableTasks_Users FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Indexes for faster queries
CREATE INDEX IX_CartableTasks_UserId ON [dbo].[CartableTasks](UserId);
CREATE INDEX IX_CartableTasks_Status ON [dbo].[CartableTasks](Status);

-- ====================================================
-- Sample Data
-- ====================================================
-- Insert roles
INSERT INTO Roles (Name) VALUES ('Admin'), ('User');

-- Insert users
INSERT INTO Users (Username, Email, PasswordSalt, PasswordHash, IsActive)
VALUES 
('reza', 'reza@example.com', 'SALT-REZA-2025', 0x66FC8389E376289DA1D290AC15847B0B84498F2A987CA942962F81511B7261E9, 1),
('admin', 'admin@example.com', 'SALT-ADMIN-2025', 0x77E6FEBDD31A6223D50C988A1D7FA1CC3ABA30A9C4AD73AE424C03C9353CD400, 1),
('ali', 'ali@example.com', 'SALT-ALI-2025', 0xD3D1043AC4C237E33C8006ABB2010F2E893AAF65AB53281E1D928AB4169B6832, 1);

-- Assign roles to users
INSERT INTO UserRoles (UserId, RoleId) VALUES
(1, 2),  -- reza = User
(2, 1),  -- admin = Admin
(3, 2);  -- ali = User

-- Insert sample tasks
INSERT INTO CartableTasks (UserId, Title, Description, Status, ReferenceNo) VALUES
(1, 'Document Review', 'Review project documents by 2025-09-10', 'New', 'REF-20250910-001'),
(1, 'Approve Request', 'Approve the pending request by team', 'Read', 'REF-20250905-002'),
(3, 'Prepare Report', 'Prepare financial report for August', 'New', 'REF-20250830-010');

-- ====================================================
-- End of script
-- ====================================================
