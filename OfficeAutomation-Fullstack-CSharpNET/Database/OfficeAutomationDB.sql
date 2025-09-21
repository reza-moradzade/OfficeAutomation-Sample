# OfficeAutomation Sample Database Script

This SQL Server script creates a **sample Office Automation database** with tables, views, and stored procedures.  
All objects are commented to help understanding and direct execution.

---

```sql
-- =======================================================
-- 0️⃣ Database Selection
-- =======================================================
USE [OfficeAutomationDB]
GO

-- =======================================================
-- 1️⃣ Table: UserSessions
-- Tracks user sessions, client type, device info, and activity timestamps
-- =======================================================
CREATE TABLE [dbo].[UserSessions](
    [SessionId] [uniqueidentifier] NOT NULL,
    [UserId] [int] NOT NULL,
    [SessionToken] [uniqueidentifier] NOT NULL,
      NOT NULL,
      NULL,
      NULL,
      NOT NULL,
      NOT NULL,
    [IsActive] [bit] NOT NULL,
    [RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_UserSessions] PRIMARY KEY CLUSTERED ([SessionId] ASC)
);
GO

-- =======================================================
-- 2️⃣ View: vw_ActiveUserSessions
-- Shows only active sessions
-- =======================================================
CREATE VIEW [dbo].[vw_ActiveUserSessions]
AS
SELECT 
    SessionId, UserId, SessionToken, ClientType, IPAddress, DeviceInfo, CreatedAt, LastActivity
FROM dbo.UserSessions
WHERE IsActive = 1;
GO

-- =======================================================
-- 3️⃣ Table: AuditLogs & AuditLogs_Archive
-- Stores user actions for auditing, with an archive table
-- =======================================================
CREATE TABLE [dbo].[AuditLogs](
    [LogId] [bigint] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
      NOT NULL,
    [Details] [nvarchar](max) NULL,
      NULL,
      NOT NULL,
 CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([LogId] ASC)
);
GO

CREATE TABLE [dbo].[AuditLogs_Archive](
    [LogId] [bigint] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
      NULL,
    [Details] [nvarchar](max) NULL,
      NULL,
      NULL,
 CONSTRAINT [PK_AuditLogs_Archive] PRIMARY KEY CLUSTERED ([LogId] ASC)
);
GO

-- =======================================================
-- 4️⃣ Table: CaptchaAttempts
-- Prevents automated login attempts
-- =======================================================
CREATE TABLE [dbo].[CaptchaAttempts](
    [CaptchaAttemptId] [bigint] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
      NULL,
      NOT NULL,
    [IsSuccess] [bit] NOT NULL,
      NOT NULL,
      NULL,
 CONSTRAINT [PK_CaptchaAttempts] PRIMARY KEY CLUSTERED ([CaptchaAttemptId] ASC)
);
GO

-- =======================================================
-- 5️⃣ Table: Cartable
-- User inbox for tasks
-- =======================================================
CREATE TABLE [dbo].[Cartable](
    [CartableId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [TaskId] [int] NOT NULL,
    [IsRead] [bit] NOT NULL,
      NOT NULL,
    [RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Cartable] PRIMARY KEY CLUSTERED ([CartableId] ASC)
);
GO

-- =======================================================
-- 6️⃣ Table: EmailVerifications
-- Stores email confirmation tokens and status
-- =======================================================
CREATE TABLE [dbo].[EmailVerifications](
    [VerificationId] [uniqueidentifier] NOT NULL,
    [UserId] [int] NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NULL,
      NULL,
 CONSTRAINT [PK_EmailVerifications] PRIMARY KEY CLUSTERED ([VerificationId] ASC)
);
GO

-- =======================================================
-- 7️⃣ Table: Users, Roles, UserRoles
-- Core user and role management
-- =======================================================
CREATE TABLE [dbo].[Users](
    [UserId] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NULL,
      NULL,
    [IsActive] [bit] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
      NOT NULL,
      NOT NULL,
    [RowVersion] [timestamp] NOT NULL,
    [IsEmailConfirmed] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);
GO

CREATE TABLE [dbo].[Roles](
    [RoleId] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC),
 CONSTRAINT [UQ_Roles_RoleName] UNIQUE ([RoleName] ASC)
);
GO

CREATE TABLE [dbo].[UserRoles](
    [UserId] [int] NOT NULL,
    [RoleId] [int] NOT NULL,
      NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId],[RoleId] ASC)
);
GO

-- =======================================================
-- 8️⃣ Table: Tasks + Files
-- Task management and file attachments
-- =======================================================
CREATE TABLE [dbo].[Tasks](
    [TaskId] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
    [Description] [nvarchar](max) NULL,
    [AssignedTo] [int] NULL,
      NOT NULL,
      NULL,
      NOT NULL,
      NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([TaskId] ASC)
);
GO

CREATE TABLE [dbo].[Files](
    [FileId] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
      NULL,
    [FileSize] [bigint] NOT NULL,
    [FileContent] [varbinary](max) NULL,
    [UploadedBy] [int] NOT NULL,
      NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([FileId] ASC)
);
GO

-- =======================================================
-- 9️⃣ Table: RefreshTokens
-- Tracks token lifecycle for authentication
-- =======================================================
CREATE TABLE [dbo].[RefreshTokens](
    [RefreshTokenId] [uniqueidentifier] NOT NULL,
    [UserId] [int] NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NULL,
      NULL,
      NULL,
      NULL,
    [IsRevoked]  AS (CASE WHEN [RevokedAt] IS NOT NULL THEN 1 ELSE 0 END),
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([RefreshTokenId] ASC)
);
GO

-- =======================================================
-- 🔑 Foreign Keys, Defaults, and Stored Procedures
-- Added for referential integrity, defaults, and task/session maintenance
-- =======================================================

-- Example stored procedure: archive old audit logs
CREATE PROCEDURE [dbo].[sp_ArchiveAuditLogs]
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

-- Cleanup expired user sessions
CREATE PROCEDURE [dbo].[sp_CleanupExpiredSessions]
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
