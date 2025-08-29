-- ============================================================
-- Office Automation Database Schema
-- Version: 1.0
-- Author: Reza Moradzade (Sample GitHub Project)
-- Description: Enterprise-Grade DB schema for Web + Windows + Mobile apps
-- ============================================================

-- ======================
-- 1. Drop old database if exists (CAUTION: removes data)
-- ======================
IF DB_ID('OfficeAutomationDB') IS NOT NULL
    DROP DATABASE OfficeAutomationDB;
GO

-- ======================
-- 2. Create Database
-- ======================
CREATE DATABASE OfficeAutomationDB;
GO

USE OfficeAutomationDB;
GO

-- ============================================================
-- 3. USERS & ROLES (Authentication + Authorization)
-- ============================================================

-- Users table with password hashing and soft-delete
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARBINARY(256) NOT NULL,
    PasswordSalt VARBINARY(128) NOT NULL,
    Email NVARCHAR(255),
    FullName NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    RowVersion ROWVERSION
);

-- Password history to prevent reuse
CREATE TABLE PasswordHistory (
    HistoryId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    OldPasswordHash VARBINARY(256) NOT NULL,
    ChangedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Roles
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL UNIQUE
);

-- User ↔ Role mapping
CREATE TABLE UserRoles (
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(RoleId),
    AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    PRIMARY KEY(UserId, RoleId)
);

-- Index for quick login lookups
CREATE UNIQUE INDEX IX_Users_UserName ON Users(UserName);

-- ============================================================
-- 4. SESSIONS (Authentication State Management)
-- ============================================================

CREATE TABLE UserSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    SessionToken UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    ClientType NVARCHAR(50) NOT NULL,  -- Web, Windows, Mobile
    IPAddress NVARCHAR(45),
    DeviceInfo NVARCHAR(255),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    LastActivity DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsActive BIT NOT NULL DEFAULT 1,
    RowVersion ROWVERSION
);

-- Prevent same user having both Web & Windows sessions simultaneously
CREATE UNIQUE INDEX IX_UserSessions_SingleClient
    ON UserSessions(UserId, ClientType)
    WHERE IsActive = 1;

-- Cleanup job recommended: expire sessions after X minutes of inactivity
CREATE INDEX IX_UserSessions_LastActivity ON UserSessions(LastActivity);

-- ============================================================
-- 5. AUDIT LOGS (Monitoring user activity)
-- ============================================================

CREATE TABLE AuditLogs (
    LogId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL FOREIGN KEY REFERENCES Users(UserId),
    Action NVARCHAR(200) NOT NULL,
    Details NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Optimized for reporting
CREATE INDEX IX_AuditLogs_Timestamp ON AuditLogs(Timestamp);
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);

-- Archiving strategy: move old data to AuditLogs_Archive
-- Partitioning by Timestamp recommended for 10M+ rows

-- ============================================================
-- 6. TASK MANAGEMENT
-- ============================================================

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

-- ============================================================
-- 7. FILE STORAGE (Documents)
-- ============================================================

CREATE TABLE Files (
    FileId INT IDENTITY(1,1) PRIMARY KEY,
    FileName NVARCHAR(255) NOT NULL,
    ContentType NVARCHAR(100),
    FileSize BIGINT NOT NULL,
    FileContent VARBINARY(MAX) NULL, -- Only for small files
    UploadedBy INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    UploadedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION
);

-- Recommendation: For large files, store path/URL instead of binary

-- ============================================================
-- 8. PERFORMANCE & MONITORING
-- ============================================================

-- Optional system stats for monitoring usage
CREATE TABLE SystemStats (
    StatId INT IDENTITY(1,1) PRIMARY KEY,
    MetricName NVARCHAR(100) NOT NULL,
    MetricValue BIGINT NOT NULL,
    RecordedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- ============================================================
-- END OF SCRIPT
-- ============================================================
