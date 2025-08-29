-- ============================================================
-- OfficeAutomationDB - Final Version
-- Author: Reza Moradzade
-- Features: Users, Roles, Sessions, Tasks, Files, Audit, SPs
-- ============================================================

-- ======================
-- 1. Drop Database if exists
-- ======================
IF DB_ID('OfficeAutomationDB') IS NOT NULL
BEGIN
    ALTER DATABASE [OfficeAutomationDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [OfficeAutomationDB];
END
GO

-- ======================
-- 2. Create Database
-- ======================
CREATE DATABASE OfficeAutomationDB;
GO

USE OfficeAutomationDB;
GO

-- ============================================================
-- 3. USERS & ROLES
-- ============================================================

-- Users table
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
GO

-- Roles table
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- UserRoles mapping
CREATE TABLE UserRoles (
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(RoleId),
    AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    PRIMARY KEY(UserId, RoleId)
);
GO

-- ============================================================
-- 4. SESSIONS
-- ============================================================

CREATE TABLE UserSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    SessionToken UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    ClientType NVARCHAR(50) NOT NULL,  -- Web / Windows / Mobile
    IPAddress NVARCHAR(45),
    DeviceInfo NVARCHAR(255),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    LastActivity DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsActive BIT NOT NULL DEFAULT 1,
    RowVersion ROWVERSION
);
GO

-- Unique index to prevent multiple active sessions per client type
CREATE UNIQUE INDEX IX_UserSessions_SingleClient
    ON UserSessions(UserId, ClientType)
    WHERE IsActive = 1;
GO

-- Index for session cleanup queries
CREATE INDEX IX_UserSessions_LastActivity ON UserSessions(LastActivity);
GO

-- ============================================================
-- 5. TASKS
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
GO

-- ============================================================
-- 6. FILES
-- ============================================================

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

CREATE TABLE AuditLogs (
    LogId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL FOREIGN KEY REFERENCES Users(UserId),
    Action NVARCHAR(200) NOT NULL,
    Details NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE INDEX IX_AuditLogs_Timestamp ON AuditLogs(Timestamp);
GO
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
GO

-- ============================================================
-- 8. FAILED LOGIN ATTEMPTS
-- ============================================================

CREATE TABLE FailedLoginAttempts (
    AttemptId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    AttemptDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IPAddress NVARCHAR(45)
);
GO

-- ============================================================
-- 9. STORED PROCEDURES
-- ============================================================

-- ---------------------------
-- sp_UserLogin
-- ---------------------------
GO
CREATE PROCEDURE sp_UserLogin
    @UserName NVARCHAR(100),
    @PasswordHash VARBINARY(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT UserId, UserName, IsActive
        FROM Users
        WHERE UserName = @UserName
          AND PasswordHash = @PasswordHash
          AND IsActive = 1;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg,16,1);
    END CATCH
END;
GO

-- ---------------------------
-- sp_UserLogout
-- ---------------------------
GO
CREATE PROCEDURE sp_UserLogout
    @SessionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE UserSessions
        SET IsActive = 0
        WHERE SessionId = @SessionId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg,16,1);
    END CATCH
END;
GO

-- ---------------------------
-- sp_KillSession (Admin)
-- ---------------------------
GO
CREATE PROCEDURE sp_KillSession
    @SessionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE UserSessions
        SET IsActive = 0
        WHERE SessionId = @SessionId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg,16,1);
    END CATCH
END;
GO

-- ---------------------------
-- sp_CleanupExpiredSessions
-- ---------------------------
GO
CREATE PROCEDURE sp_CleanupExpiredSessions
    @InactivityMinutes INT = 30
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE UserSessions
        SET IsActive = 0
        WHERE DATEDIFF(MINUTE, LastActivity, SYSUTCDATETIME()) > @InactivityMinutes;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg,16,1);
    END CATCH
END;
GO

-- ---------------------------
-- sp_ArchiveAuditLogs
-- ---------------------------
GO
CREATE TABLE AuditLogs_Archive (
    LogId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL,
    Action NVARCHAR(200),
    Details NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    Timestamp DATETIME2
);
GO

GO
CREATE PROCEDURE sp_ArchiveAuditLogs
    @BeforeDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO AuditLogs_Archive (UserId, Action, Details, IPAddress, Timestamp)
        SELECT UserId, Action, Details, IPAddress, Timestamp
        FROM AuditLogs
        WHERE Timestamp < @BeforeDate;

        DELETE FROM AuditLogs
        WHERE Timestamp < @BeforeDate;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg,16,1);
    END CATCH
END;
GO

-- ============================================================
-- 10. SAMPLE DATA
-- ============================================================

-- Roles
INSERT INTO Roles(RoleName) VALUES ('Admin'),('User');
GO

-- Users
INSERT INTO Users(UserName, PasswordSalt, PasswordHash, Email, FullName)
VALUES 
('reza', CAST('SALT-REZA-2025' AS VARBINARY(128)), 0x66FC8389E376289DA1D290AC15847B0B84498F2A987CA942962F81511B7261E9, 'reza@example.com', 'Reza M'),
('admin', CAST('SALT-ADMIN-2025' AS VARBINARY(128)), 0x77E6FEBDD31A6223D50C988A1D7FA1CC3ABA30A9C4AD73AE424C03C9353CD400, 'admin@example.com', 'Administrator');
GO

-- UserRoles
INSERT INTO UserRoles(UserId, RoleId) VALUES (1,2),(2,1);
GO

-- Tasks
INSERT INTO Tasks(Title, Description, AssignedTo, Status)
VALUES ('Document Review','Review project documents',1,'Pending');
GO

-- Files
INSERT INTO Files(FileName, FileSize, UploadedBy)
VALUES ('SampleDoc.pdf',102400,1);
GO

-- ============================================================
-- ✅ END OF SCRIPT
-- ============================================================
