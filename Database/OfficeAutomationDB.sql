USE [master]
GO
/****** Object:  Database [OfficeAutomationDB]    Script Date: 9/1/2025 8:05:27 PM ******/
CREATE DATABASE [OfficeAutomationDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OfficeAutomationDB', FILENAME = N'C:\Users\Fujitsu\OfficeAutomationDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OfficeAutomationDB_log', FILENAME = N'C:\Users\Fujitsu\OfficeAutomationDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [OfficeAutomationDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OfficeAutomationDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OfficeAutomationDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [OfficeAutomationDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OfficeAutomationDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OfficeAutomationDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [OfficeAutomationDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OfficeAutomationDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [OfficeAutomationDB] SET  MULTI_USER 
GO
ALTER DATABASE [OfficeAutomationDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OfficeAutomationDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OfficeAutomationDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OfficeAutomationDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OfficeAutomationDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OfficeAutomationDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [OfficeAutomationDB] SET QUERY_STORE = OFF
GO
USE [OfficeAutomationDB]
GO
/****** Object:  Table [dbo].[UserSessions]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSessions](
	[SessionId] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
	[SessionToken] [uniqueidentifier] NOT NULL,
	[ClientType] [nvarchar](50) NOT NULL,
	[IPAddress] [nvarchar](45) NULL,
	[DeviceInfo] [nvarchar](255) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[LastActivity] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_ActiveUserSessions]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ایجاد View برای نمایش نشست‌های فعال کاربران
CREATE VIEW [dbo].[vw_ActiveUserSessions]
AS
SELECT 
    SessionId, 
    UserId, 
    SessionToken, 
    ClientType, 
    IPAddress, 
    DeviceInfo, 
    CreatedAt, 
    LastActivity
FROM dbo.UserSessions
WHERE IsActive = 1;
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Action] [nvarchar](200) NOT NULL,
	[Details] [nvarchar](max) NULL,
	[IPAddress] [nvarchar](45) NULL,
	[Timestamp] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs_Archive]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs_Archive](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Action] [nvarchar](200) NULL,
	[Details] [nvarchar](max) NULL,
	[IPAddress] [nvarchar](45) NULL,
	[Timestamp] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaptchaAttempts]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaptchaAttempts](
	[CaptchaAttemptId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[IPAddress] [nvarchar](45) NULL,
	[Action] [nvarchar](100) NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[AttemptAt] [datetime2](7) NOT NULL,
	[Details] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[CaptchaAttemptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cartable]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cartable](
	[CartableId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TaskId] [int] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[ReceivedAt] [datetime2](7) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CartableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailVerifications]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailVerifications](
	[VerificationId] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
	[Token] [nvarchar](450) NOT NULL,
	[ExpiresAt] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ConfirmedAt] [datetime2](7) NULL,
	[ConfirmIp] [nvarchar](45) NULL,
PRIMARY KEY CLUSTERED 
(
	[VerificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FailedLoginAttempts]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FailedLoginAttempts](
	[AttemptId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[AttemptDate] [datetime2](7) NOT NULL,
	[IPAddress] [nvarchar](45) NULL,
PRIMARY KEY CLUSTERED 
(
	[AttemptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Files]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[ContentType] [nvarchar](100) NULL,
	[FileSize] [bigint] NOT NULL,
	[FileContent] [varbinary](max) NULL,
	[UploadedBy] [int] NOT NULL,
	[UploadedAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[RefreshTokenId] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
	[Token] [nvarchar](450) NOT NULL,
	[ExpiresAt] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedByIp] [nvarchar](45) NULL,
	[RevokedAt] [datetime2](7) NULL,
	[RevokedByIp] [nvarchar](45) NULL,
	[ReplacedByToken] [nvarchar](450) NULL,
	[IsRevoked]  AS (case when [RevokedAt] IS NOT NULL then (1) else (0) end),
PRIMARY KEY CLUSTERED 
(
	[RefreshTokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[AssignedTo] [int] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[DueDate] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[AssignedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[PasswordHash] [varbinary](256) NOT NULL,
	[PasswordSalt] [varbinary](128) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[FullName] [nvarchar](200) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[IsEmailConfirmed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CaptchaAttempts] ON 

INSERT [dbo].[CaptchaAttempts] ([CaptchaAttemptId], [UserId], [IPAddress], [Action], [IsSuccess], [AttemptAt], [Details]) VALUES (1, 1, N'127.0.0.1', N'login', 1, CAST(N'2025-09-01T15:57:41.1655197' AS DateTime2), N'Captcha passed successfully')
INSERT [dbo].[CaptchaAttempts] ([CaptchaAttemptId], [UserId], [IPAddress], [Action], [IsSuccess], [AttemptAt], [Details]) VALUES (2, 2, N'127.0.0.2', N'login', 0, CAST(N'2025-09-01T15:57:41.1655197' AS DateTime2), N'Captcha failed - incorrect characters')
SET IDENTITY_INSERT [dbo].[CaptchaAttempts] OFF
GO
SET IDENTITY_INSERT [dbo].[Cartable] ON 

INSERT [dbo].[Cartable] ([CartableId], [UserId], [TaskId], [IsRead], [ReceivedAt]) VALUES (1, 1, 1, 0, CAST(N'2025-09-01T15:56:17.5774774' AS DateTime2))
INSERT [dbo].[Cartable] ([CartableId], [UserId], [TaskId], [IsRead], [ReceivedAt]) VALUES (2, 1, 1, 0, CAST(N'2025-09-01T16:04:12.4350666' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Cartable] OFF
GO
INSERT [dbo].[EmailVerifications] ([VerificationId], [UserId], [Token], [ExpiresAt], [CreatedAt], [ConfirmedAt], [ConfirmIp]) VALUES (N'bdd0a5b2-ee85-4908-b340-4941a0cb3ebb', 1, N'EMAIL-VERIFICATION-TOKEN-REZA', CAST(N'2025-09-02T15:57:06.1288749' AS DateTime2), CAST(N'2025-09-01T15:57:06.1288749' AS DateTime2), NULL, NULL)
INSERT [dbo].[EmailVerifications] ([VerificationId], [UserId], [Token], [ExpiresAt], [CreatedAt], [ConfirmedAt], [ConfirmIp]) VALUES (N'9ca54162-1f52-40b1-9d83-c32134911f19', 2, N'EMAIL-VERIFICATION-TOKEN-ADMIN', CAST(N'2025-09-02T15:57:06.1288749' AS DateTime2), CAST(N'2025-09-01T15:57:06.1288749' AS DateTime2), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Files] ON 

INSERT [dbo].[Files] ([FileId], [FileName], [ContentType], [FileSize], [FileContent], [UploadedBy], [UploadedAt], [IsDeleted]) VALUES (1, N'SampleDoc.pdf', NULL, 102400, NULL, 1, CAST(N'2025-08-30T19:17:41.2786997' AS DateTime2), 0)
INSERT [dbo].[Files] ([FileId], [FileName], [ContentType], [FileSize], [FileContent], [UploadedBy], [UploadedAt], [IsDeleted]) VALUES (2, N'SampleDoc.pdf', NULL, 102400, NULL, 1, CAST(N'2025-09-01T16:04:12.4420481' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Files] OFF
GO
INSERT [dbo].[RefreshTokens] ([RefreshTokenId], [UserId], [Token], [ExpiresAt], [CreatedAt], [CreatedByIp], [RevokedAt], [RevokedByIp], [ReplacedByToken]) VALUES (N'a99e339a-b649-4739-8ac8-88364aa8b7b8', 1, N'REFRESH-TOKEN-REZA-001', CAST(N'2025-09-08T15:54:03.9630903' AS DateTime2), CAST(N'2025-09-01T15:54:03.9630903' AS DateTime2), N'127.0.0.1', NULL, NULL, NULL)
INSERT [dbo].[RefreshTokens] ([RefreshTokenId], [UserId], [Token], [ExpiresAt], [CreatedAt], [CreatedByIp], [RevokedAt], [RevokedByIp], [ReplacedByToken]) VALUES (N'62de1dde-63f7-41b6-8115-febc5285a1f7', 2, N'REFRESH-TOKEN-ADMIN-001', CAST(N'2025-09-08T15:54:03.9630903' AS DateTime2), CAST(N'2025-09-01T15:54:03.9630903' AS DateTime2), N'127.0.0.1', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (2, N'User')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Tasks] ON 

INSERT [dbo].[Tasks] ([TaskId], [Title], [Description], [AssignedTo], [Status], [DueDate], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (1, N'Document Review', N'Review project documents', 1, N'Pending', NULL, CAST(N'2025-08-30T19:17:41.2753043' AS DateTime2), CAST(N'2025-08-30T19:17:41.2753043' AS DateTime2), 0)
INSERT [dbo].[Tasks] ([TaskId], [Title], [Description], [AssignedTo], [Status], [DueDate], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (2, N'Document Review', N'Review project documents', 1, N'Pending', NULL, CAST(N'2025-09-01T16:04:12.4312373' AS DateTime2), CAST(N'2025-09-01T16:04:12.4312373' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Tasks] OFF
GO
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [AssignedAt]) VALUES (1, 2, CAST(N'2025-08-30T19:17:41.2723355' AS DateTime2))
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [AssignedAt]) VALUES (2, 1, CAST(N'2025-08-30T19:17:41.2723355' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [UserName], [PasswordHash], [PasswordSalt], [Email], [FullName], [IsActive], [IsDeleted], [CreatedAt], [UpdatedAt], [IsEmailConfirmed]) VALUES (1, N'reza', 0xA6C6C7ED8FC1068B7A70A41DA722F75D83E6CE0E451FC80BA0F2AFDD04D101DA, 0x53414C542D52455A412D32303235, N'reza@example.com', N'Reza M', 1, 0, CAST(N'2025-08-30T19:17:41.2643155' AS DateTime2), CAST(N'2025-08-30T19:17:41.2643155' AS DateTime2), 0)
INSERT [dbo].[Users] ([UserId], [UserName], [PasswordHash], [PasswordSalt], [Email], [FullName], [IsActive], [IsDeleted], [CreatedAt], [UpdatedAt], [IsEmailConfirmed]) VALUES (2, N'admin', 0x77E6FEBDD31A6223D50C988A1D7FA1CC3ABA30A9C4AD73AE424C03C9353CD400, 0x53414C542D41444D494E2D32303235, N'admin@example.com', N'Administrator', 1, 0, CAST(N'2025-08-30T19:17:41.2643155' AS DateTime2), CAST(N'2025-08-30T19:17:41.2643155' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_AuditLogs_Timestamp]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_AuditLogs_Timestamp] ON [dbo].[AuditLogs]
(
	[Timestamp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuditLogs_UserId]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_AuditLogs_UserId] ON [dbo].[AuditLogs]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cartable_UserId_IsRead_ReceivedAt]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_Cartable_UserId_IsRead_ReceivedAt] ON [dbo].[Cartable]
(
	[UserId] ASC,
	[IsRead] ASC,
	[ReceivedAt] ASC
)
INCLUDE([TaskId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Files_UploadedBy]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_Files_UploadedBy] ON [dbo].[Files]
(
	[UploadedBy] ASC
)
INCLUDE([FileName],[FileSize],[UploadedAt]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RefreshTokens_UserId_Token]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RefreshTokens_UserId_Token] ON [dbo].[RefreshTokens]
(
	[UserId] ASC,
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B616058A4E3C5]    Script Date: 9/1/2025 8:05:28 PM ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Tasks_AssignedTo_Status]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_Tasks_AssignedTo_Status] ON [dbo].[Tasks]
(
	[AssignedTo] ASC,
	[Status] ASC
)
INCLUDE([Title],[DueDate],[CreatedAt]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__C9F28456F5BF4196]    Script Date: 9/1/2025 8:05:28 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Users_UserName_Active]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Users_UserName_Active] ON [dbo].[Users]
(
	[UserName] ASC
)
WHERE ([IsActive]=(1) AND [IsDeleted]=(0))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserSessions_LastActivity]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserSessions_LastActivity] ON [dbo].[UserSessions]
(
	[LastActivity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserSessions_SingleClient]    Script Date: 9/1/2025 8:05:28 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserSessions_SingleClient] ON [dbo].[UserSessions]
(
	[UserId] ASC,
	[ClientType] ASC
)
WHERE ([IsActive]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditLogs] ADD  DEFAULT (sysutcdatetime()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[CaptchaAttempts] ADD  DEFAULT (sysutcdatetime()) FOR [AttemptAt]
GO
ALTER TABLE [dbo].[Cartable] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[Cartable] ADD  DEFAULT (sysutcdatetime()) FOR [ReceivedAt]
GO
ALTER TABLE [dbo].[EmailVerifications] ADD  DEFAULT (newid()) FOR [VerificationId]
GO
ALTER TABLE [dbo].[EmailVerifications] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[FailedLoginAttempts] ADD  DEFAULT (sysutcdatetime()) FOR [AttemptDate]
GO
ALTER TABLE [dbo].[Files] ADD  DEFAULT (sysutcdatetime()) FOR [UploadedAt]
GO
ALTER TABLE [dbo].[Files] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RefreshTokens] ADD  DEFAULT (newid()) FOR [RefreshTokenId]
GO
ALTER TABLE [dbo].[RefreshTokens] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (sysutcdatetime()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (sysutcdatetime()) FOR [AssignedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (sysutcdatetime()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsEmailConfirmed]
GO
ALTER TABLE [dbo].[UserSessions] ADD  DEFAULT (newid()) FOR [SessionId]
GO
ALTER TABLE [dbo].[UserSessions] ADD  DEFAULT (newid()) FOR [SessionToken]
GO
ALTER TABLE [dbo].[UserSessions] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserSessions] ADD  DEFAULT (sysutcdatetime()) FOR [LastActivity]
GO
ALTER TABLE [dbo].[UserSessions] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AuditLogs]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[CaptchaAttempts]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Cartable]  WITH CHECK ADD FOREIGN KEY([TaskId])
REFERENCES [dbo].[Tasks] ([TaskId])
GO
ALTER TABLE [dbo].[Cartable]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[EmailVerifications]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[FailedLoginAttempts]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD FOREIGN KEY([UploadedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD FOREIGN KEY([AssignedTo])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserSessions]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
/****** Object:  StoredProcedure [dbo].[sp_ArchiveAuditLogs]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  StoredProcedure [dbo].[sp_CleanupExpiredSessions]    Script Date: 9/1/2025 8:05:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
USE [master]
GO
ALTER DATABASE [OfficeAutomationDB] SET  READ_WRITE 
GO
