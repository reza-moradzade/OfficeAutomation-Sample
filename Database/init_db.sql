/* ===========================================================
   Project: OfficeAutomation-Sample
   File: init_db.sql
   Description:
       Master script for creating the Office Automation database,
       all tables, relationships, constraints, and inserting
       sample data for demo purposes.
   Author: reza moradzade
   =========================================================== */

---------------------------------------------------------------
-- 1. Drop Database if it already exists (for clean setup)
---------------------------------------------------------------
IF DB_ID('OfficeAutomationDB') IS NOT NULL
BEGIN
    ALTER DATABASE OfficeAutomationDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE OfficeAutomationDB;
END
GO

---------------------------------------------------------------
-- 2. Create Database
---------------------------------------------------------------
CREATE DATABASE OfficeAutomationDB;
GO

USE OfficeAutomationDB;
GO

---------------------------------------------------------------
-- 3. Create Tables
---------------------------------------------------------------

-- Table: Roles
-- Purpose: Defines user roles in the system (Admin, User, etc.)
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

-- Table: Users
-- Purpose: Stores user accounts and authentication details
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL, -- Note: Hash in real projects
    FullName NVARCHAR(100) NOT NULL,
    RoleId INT NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- Table: Kartable
-- Purpose: Represents incoming/outgoing tasks (letters, forms, etc.)
CREATE TABLE Kartable (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    AssignedTo INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    CONSTRAINT FK_Kartable_Users FOREIGN KEY (AssignedTo) REFERENCES Users(UserId)
);

---------------------------------------------------------------
-- 4. Insert Initial Data (Seed)
---------------------------------------------------------------

-- Insert Roles
INSERT INTO Roles (RoleName) VALUES 
(N'Admin'),
(N'User');

-- Insert Sample Users
-- Note: Passwords are plain text for demo only. Use hashing in production!
INSERT INTO Users (Username, PasswordHash, FullName, RoleId) VALUES
(N'admin', N'1234', N'System Administrator', 1),
(N'user1', N'1234', N'First Demo User', 2);

-- Insert Sample Kartable Records
INSERT INTO Kartable (Title, Description, AssignedTo, Status) VALUES
(N'Letter 1', N'Sample description for letter 1', 2, N'Pending'),
(N'Letter 2', N'Sample description for letter 2', 2, N'Done'),
(N'Letter 3', N'Assigned to user1 as pending task', 2, N'Pending');

---------------------------------------------------------------
-- 5. Final Notes
---------------------------------------------------------------
-- This script creates a clean OfficeAutomationDB database with:
--   - Roles (Admin/User)
--   - Users (Admin + one normal user)
--   - Kartable (demo tasks)
-- 
-- Suitable for portfolio/demo purposes.
-- Replace plain passwords with hashed values in production systems.
---------------------------------------------------------------

