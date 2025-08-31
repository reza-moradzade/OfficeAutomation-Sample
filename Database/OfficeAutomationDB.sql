-- ============================================================
-- OfficeAutomationDB - Final Clean Version with Comments
-- Author: Reza Moradzade
-- Purpose: Sample Office Automation Database for learning and demo
-- Features: Users, Roles, Sessions, RefreshTokens, Tasks, Cartable,
--           Files, AuditLogs, Failed Login Attempts, Email Verification,
--           Captcha Attempts, Indexes for performance
-- ============================================================

-- ======================
-- 1. Drop Database if exists
-- ======================
-- This ensures a clean start. Any existing OfficeAutomationDB will be dropped.
IF DB_ID('OfficeAutomationDB') IS NOT NULL
BEGIN
    ALTER DATABASE [OfficeAutomationDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [OfficeAutomationDB];
END
GO

-- ======================
-- 2. Create Database
-- ======================
-- Creating the main database for the sample automation system
CREATE DATABASE OfficeAutomationDB;
GO

USE OfficeAutomationDB;
GO

-- ============================================================
-- 3. USERS & ROLES
-- ============================================================
-- Users table: stores user information, login credentials, email confirmation, and active status
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARBINARY(256) NOT NULL,
    PasswordSalt VARBINARY(128) NOT NULL,
    Email NVARCHAR(255),
    IsEmailConfirmed BIT NOT NULL DEFAULT 0,
    FullName NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    RowVersion ROWVERSION
);
GO

-- Roles table: defines user roles such as Admin or User
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- UserRoles table: many-to-many mapping between Users and Roles
CREATE TABLE UserRoles (
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(RoleId),
    AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    PRIMARY KEY(UserId, RoleId)
);
GO

-- ============================================================
-- 4. SESSIONS & TOKENS
-- ============================================================
-- UserSessions table: tracks active sessions per client (web, windows, mobile)
CREATE TABLE UserSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    SessionToken UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    ClientType NVARCHAR(50) NOT NULL,
    IPAddress NVARCHAR(45),
    DeviceInfo NVARCHAR(255),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    LastActivity DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsActive BIT NOT NULL DEFAULT 1,
    RowVersion ROWVERSION
);
GO

-- RefreshTokens table: stores refresh tokens for secure authentication
CREATE TABLE RefreshTokens (
    RefreshTokenId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    Token NVARCHAR(450) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedByIp NVARCHAR(45),
    RevokedAt DATETIME2 NULL,
    RevokedByIp NVARCHAR(45),
    ReplacedByToken NVARCHAR(450) NULL,
    -- IsRevoked computed column (calculated in query or API, not persisted)
    IsRevoked AS (CASE WHEN RevokedAt IS NOT NULL THEN 1 ELSE 0 END)
);
GO

-- Index to ensure a user cannot have duplicate tokens
CREATE UNIQUE INDEX IX_RefreshTokens_UserId_Token ON RefreshTokens(UserId, Token);
GO

-- Unique index to allow only one active session per client type
CREATE UNIQUE INDEX IX_UserSessions_SingleClient
    ON UserSessions(UserId, ClientType)
    WHERE IsActive = 1;
GO

-- Index to speed up queries filtering by last activity
CREATE INDEX IX_UserSessions_LastActivity ON UserSessions(LastActivity);
GO

-- ============================================================
-- 5. TASKS & CARTABLE
-- ============================================================
-- Tasks table: stores tasks, status, assigned user, and metadata
CREATE TABLE Tasks (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    AssignedTo INT NULL FOREIGN KEY REFERENCES Users(UserId),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    DueDate DATETIME2,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION
);
GO

-- Cartable table: stores user-specific task assignments (inbox)
CREATE TABLE Cartable (
    CartableId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    TaskId INT NOT NULL FOREIGN KEY REFERENCES Tasks(TaskId),
    IsRead BIT NOT NULL DEFAULT 0,
    ReceivedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    RowVersion ROWVERSION
);
GO

-- ============================================================
-- 6. FILES
-- ============================================================
-- Files table: stores uploaded documents for tasks or projects
CREATE TABLE Files (
    FileId INT IDENTITY(1,1) PRIMARY KEY,
    FileName NVARCHAR(255) NOT NULL,
    ContentType NVARCHAR(100),
    FileSize BIGINT NOT NULL,
    FileContent VARBINARY(MAX) NULL,
    UploadedBy INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    UploadedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION
);
GO

-- ============================================================
-- 7. AUDIT LOGS
-- ============================================================
-- AuditLogs table: tracks user actions for monitoring and security
CREATE TABLE AuditLogs (
    LogId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL FOREIGN KEY REFERENCES Users(UserId),
    Action NVARCHAR(200) NOT NULL,
    Details NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- Indexes to speed up audit queries
CREATE INDEX IX_AuditLogs_Timestamp ON AuditLogs(Timestamp);
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
GO

-- Archived audit logs
CREATE TABLE AuditLogs_Archive (
    LogId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL,
    Action NVARCHAR(200),
    Details NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    Timestamp DATETIME2
);
GO

-- ============================================================
-- 8. FAILED LOGIN ATTEMPTS
-- ============================================================
-- Tracks failed login attempts for security monitoring and brute-force prevention
CREATE TABLE FailedLoginAttempts (
    AttemptId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    AttemptDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IPAddress NVARCHAR(45)
);
GO

CREATE NONCLUSTERED INDEX IX_FailedLoginAttempts_UserId_AttemptDate
ON FailedLoginAttempts(UserId, AttemptDate);
GO

-- ============================================================
-- 9. EMAIL VERIFICATION & CAPTCHA
-- ============================================================
-- EmailVerifications table: stores verification tokens for email confirmation
CREATE TABLE EmailVerifications (
    VerificationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    Token NVARCHAR(450) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ConfirmedAt DATETIME2 NULL,
    ConfirmIp NVARCHAR(45) NULL
);
GO

-- CaptchaAttempts table: stores CAPTCHA results for login/signup
CREATE TABLE CaptchaAttempts (
    CaptchaAttemptId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL FOREIGN KEY REFERENCES Users(UserId),
    IPAddress NVARCHAR(45) NULL,
    Action NVARCHAR(100) NOT NULL,
    IsSuccess BIT NOT NULL,
    AttemptAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Details NVARCHAR(1000) NULL
);
GO

-- ============================================================
-- 10. INDEXES FOR PERFORMANCE
-- ============================================================
CREATE NONCLUSTERED INDEX IX_Tasks_AssignedTo_Status
ON Tasks(AssignedTo, Status)
INCLUDE (Title, DueDate, CreatedAt);
GO

CREATE NONCLUSTERED INDEX IX_Cartable_UserId_IsRead_ReceivedAt
ON Cartable(UserId, IsRead, ReceivedAt)
INCLUDE (TaskId);
GO

CREATE NONCLUSTERED INDEX IX_Files_UploadedBy
ON Files(UploadedBy)
INCLUDE (FileName, FileSize, UploadedAt);
GO

CREATE UNIQUE NONCLUSTERED INDEX UX_Users_UserName_Active
ON Users(UserName)
WHERE IsActive = 1 AND IsDeleted = 0;
GO

-- ============================================================
-- 11. VIEW
-- ============================================================
-- Shows all active user sessions
CREATE VIEW vw_ActiveUserSessions
AS
SELECT SessionId, UserId, SessionToken, ClientType, IPAddress, DeviceInfo, CreatedAt, LastActivity
FROM UserSessions
WHERE IsActive = 1;
GO

-- ============================================================
-- 12. SAMPLE DATA
-- ============================================================
-- Insert sample roles
INSERT INTO Roles(RoleName) VALUES ('Admin'),('User');
GO

-- Insert sample users with hashed passwords
INSERT INTO Users(UserName, PasswordSalt, PasswordHash, Email, FullName)
VALUES 
('reza', CAST('SALT-REZA-2025' AS VARBINARY(128)), 0x66FC8389E376289DA1D290AC15847B0B84498F2A987CA942962F81511B7261E9, 'reza@example.com', 'Reza M'),
('admin', CAST('SALT-ADMIN-2025' AS VARBINARY(128)), 0x77E6FEBDD31A6223D50C988A1D7FA1CC3ABA30A9C4AD73AE424C03C9353CD400, 'admin@example.com', 'Administrator');
GO

-- Assign roles to users
INSERT INTO UserRoles(UserId, RoleId) VALUES (1,2),(2,1);
GO

-- Insert sample task
INSERT INTO Tasks(Title, Description, AssignedTo, Status)
VALUES ('Document Review','Review project documents',1,'Pending');
GO

-- Assign task to user cartable
INSERT INTO Cartable(UserId, TaskId) VALUES (1,1);
GO

-- Insert sample file
INSERT INTO Files(FileName, FileSize, UploadedBy)
VALUES ('SampleDoc.pdf',102400,1);
GO
