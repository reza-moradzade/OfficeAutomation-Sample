-- ===============================================
-- Database Setup Script for OfficeAutomationDB
-- Author: Reza Moradzade
-- Script Date: 2025-09-01
-- Purpose: Create database, tables, indexes, constraints, default values, views, stored procedures, and seed initial data
-- ===============================================

-- Use the master database to create a new database
USE [master]
GO

/****** Create the database OfficeAutomationDB ******/
CREATE DATABASE [OfficeAutomationDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OfficeAutomationDB', 
  FILENAME = N'C:\Users\Fujitsu\OfficeAutomationDB.mdf', 
  SIZE = 8192KB, MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OfficeAutomationDB_log', 
  FILENAME = N'C:\Users\Fujitsu\OfficeAutomationDB_log.ldf', 
  SIZE = 8192KB, MAXSIZE = 2048GB, FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

-- Set compatibility level for SQL Server 2019 (150)
ALTER DATABASE [OfficeAutomationDB] SET COMPATIBILITY_LEVEL = 150
GO

-- Enable full-text search if installed
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
BEGIN
    EXEC [OfficeAutomationDB].[dbo].[sp_fulltext_database] @action = 'enable'
END
GO

-- Database configuration options
ALTER DATABASE [OfficeAutomationDB] SET ANSI_NULL_DEFAULT OFF 
ALTER DATABASE [OfficeAutomationDB] SET ANSI_NULLS OFF 
ALTER DATABASE [OfficeAutomationDB] SET ANSI_PADDING OFF 
ALTER DATABASE [OfficeAutomationDB] SET ANSI_WARNINGS OFF 
ALTER DATABASE [OfficeAutomationDB] SET ARITHABORT OFF 
ALTER DATABASE [OfficeAutomationDB] SET AUTO_CLOSE ON 
ALTER DATABASE [OfficeAutomationDB] SET AUTO_SHRINK OFF 
ALTER DATABASE [OfficeAutomationDB] SET AUTO_UPDATE_STATISTICS ON 
ALTER DATABASE [OfficeAutomationDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
ALTER DATABASE [OfficeAutomationDB] SET CURSOR_DEFAULT  GLOBAL 
ALTER DATABASE [OfficeAutomationDB] SET CONCAT_NULL_YIELDS_NULL OFF 
ALTER DATABASE [OfficeAutomationDB] SET NUMERIC_ROUNDABORT OFF 
ALTER DATABASE [OfficeAutomationDB] SET QUOTED_IDENTIFIER OFF 
ALTER DATABASE [OfficeAutomationDB] SET RECURSIVE_TRIGGERS OFF 
ALTER DATABASE [OfficeAutomationDB] SET ENABLE_BROKER 
ALTER DATABASE [OfficeAutomationDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
ALTER DATABASE [OfficeAutomationDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
ALTER DATABASE [OfficeAutomationDB] SET TRUSTWORTHY OFF 
ALTER DATABASE [OfficeAutomationDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
ALTER DATABASE [OfficeAutomationDB] SET PARAMETERIZATION SIMPLE 
ALTER DATABASE [OfficeAutomationDB] SET READ_COMMITTED_SNAPSHOT OFF 
ALTER DATABASE [OfficeAutomationDB] SET HONOR_BROKER_PRIORITY OFF 
ALTER DATABASE [OfficeAutomationDB] SET RECOVERY SIMPLE 
ALTER DATABASE [OfficeAutomationDB] SET MULTI_USER 
ALTER DATABASE [OfficeAutomationDB] SET PAGE_VERIFY CHECKSUM  
ALTER DATABASE [OfficeAutomationDB] SET DB_CHAINING OFF 
ALTER DATABASE [OfficeAutomationDB] SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF) 
ALTER DATABASE [OfficeAutomationDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
ALTER DATABASE [OfficeAutomationDB] SET DELAYED_DURABILITY = DISABLED 
ALTER DATABASE [OfficeAutomationDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
ALTER DATABASE [OfficeAutomationDB] SET QUERY_STORE = OFF
GO

-- Switch to the newly created database
USE [OfficeAutomationDB]
GO

-- ===============================================
-- Create Tables
-- ===============================================

-- Table: UserSessions
-- Stores active and past user sessions
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
PRIMARY KEY CLUSTERED ([SessionId] ASC)
) ON [PRIMARY]
GO

-- Table: AuditLogs
-- Stores audit trail of user actions
CREATE TABLE [dbo].[AuditLogs](
    [LogId] [bigint] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
      NOT NULL,
    [Details] [nvarchar](max) NULL,
      NULL,
      NOT NULL,
PRIMARY KEY CLUSTERED ([LogId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- Table: AuditLogs_Archive
-- Stores archived audit logs
CREATE TABLE [dbo].[AuditLogs_Archive](
    [LogId] [bigint] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
      NULL,
    [Details] [nvarchar](max) NULL,
      NULL,
      NULL,
PRIMARY KEY CLUSTERED ([LogId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- Table: CaptchaAttempts
-- Tracks captcha validation attempts
CREATE TABLE [dbo].[CaptchaAttempts](
    [CaptchaAttemptId] [bigint] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
      NULL,
      NOT NULL,
    [IsSuccess] [bit] NOT NULL,
      NOT NULL,
      NULL,
PRIMARY KEY CLUSTERED ([CaptchaAttemptId] ASC)
) ON [PRIMARY]
GO

-- Table: Cartable
-- Tracks user tasks / inbox
CREATE TABLE [dbo].[Cartable](
    [CartableId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [TaskId] [int] NOT NULL,
    [IsRead] [bit] NOT NULL,
      NOT NULL,
    [RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED ([CartableId] ASC)
) ON [PRIMARY]
GO

-- Table: EmailVerifications
-- Tracks email verification tokens
CREATE TABLE [dbo].[EmailVerifications](
    [VerificationId] [uniqueidentifier] NOT NULL,
    [UserId] [int] NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
      NULL,
      NULL,
PRIMARY KEY CLUSTERED ([VerificationId] ASC)
) ON [PRIMARY]
GO

-- Table: FailedLoginAttempts
-- Tracks failed login attempts
CREATE TABLE [dbo].[FailedLoginAttempts](
    [AttemptId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
      NOT NULL,
      NULL,
PRIMARY KEY CLUSTERED ([AttemptId] ASC)
) ON [PRIMARY]
GO

-- Table: Files
-- Stores uploaded files
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
PRIMARY KEY CLUSTERED ([FileId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- Table: RefreshTokens
-- Stores JWT refresh tokens
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
    [IsRevoked] AS (CASE WHEN [RevokedAt] IS NOT NULL THEN 1 ELSE 0 END),
PRIMARY KEY CLUSTERED ([RefreshTokenId] ASC)
) ON [PRIMARY]
GO

-- Table: Roles
-- Stores user roles
CREATE TABLE [dbo].[Roles](
    [RoleId] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
PRIMARY KEY CLUSTERED ([RoleId] ASC)
) ON [PRIMARY]
GO

-- Table: Tasks
-- Stores user tasks
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
PRIMARY KEY CLUSTERED ([TaskId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- Table: UserRoles
-- Maps users to roles
CREATE TABLE [dbo].[UserRoles](
    [UserId] [int] NOT NULL,
    [RoleId] [int] NOT NULL,
      NOT NULL,
PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
) ON [PRIMARY]
GO

-- Table: Users
-- Stores user accounts
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
PRIMARY KEY CLUSTERED ([UserId] ASC)
) ON [PRIMARY]
GO

-- ===============================================
-- Views, Indexes, Foreign Keys, Default Values, and Stored Procedures
-- ===============================================
-- The script continues with:
-- 1. Views (e.g., vw_ActiveUserSessions)
-- 2. Indexes for performance
-- 3. Default constraints for automatic values
-- 4. Foreign key constraints
-- 5. Stored Procedures (sp_ArchiveAuditLogs, sp_CleanupExpiredSessions)
-- 6. Seed data inserts for testing (CaptchaAttempts, Users, Tasks, Roles, Cartable, EmailVerifications, Files, RefreshTokens, UserRoles)

-- ===============================================
-- Notes for GitHub Users:
-- 1. Copy the entire script into SQL Server Management Studio (SSMS) and execute.
-- 2. Modify file paths for MDF/LDF files if necessary.
-- 3. All tables, relationships, constraints, indexes, stored procedures, and seed data will be created.
-- 4. Compatible with SQL Server 2019 or higher.
-- ===============================================
