USE [master]
GO
/****** Object:  Database [FATMSDB]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE DATABASE [FATMSDB]
GO
ALTER DATABASE [FATMSDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FATMSDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FATMSDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FATMSDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FATMSDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [FATMSDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FATMSDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FATMSDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FATMSDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FATMSDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FATMSDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FATMSDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FATMSDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FATMSDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FATMSDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FATMSDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FATMSDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FATMSDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FATMSDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FATMSDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FATMSDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [FATMSDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FATMSDB] SET RECOVERY FULL 
GO
ALTER DATABASE [FATMSDB] SET  MULTI_USER 
GO
ALTER DATABASE [FATMSDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FATMSDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FATMSDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FATMSDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FATMSDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FATMSDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'FATMSDB', N'ON'
GO
ALTER DATABASE [FATMSDB] SET QUERY_STORE = OFF
GO
USE [FATMSDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attendances]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendances](
	[ClassUserId] [int] NOT NULL,
	[Day] [datetime2](7) NOT NULL,
	[MorningStatus] [int] NULL,
	[NoonStatus] [int] NULL,
	[NightStatus] [int] NULL,
	[Reason] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_Attendances] PRIMARY KEY CLUSTERED 
(
	[ClassUserId] ASC,
	[Day] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditDetails]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Feedback] [nvarchar](500) NOT NULL,
	[Rating] [int] NOT NULL,
	[PlanId] [int] NOT NULL,
	[AuditorId] [int] NOT NULL,
	[TraineeId] [int] NOT NULL,
 CONSTRAINT [PK_AuditDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditPlans]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditPlans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuditDate] [datetime2](7) NOT NULL,
	[PlannedBy] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[SyllabusId] [int] NOT NULL,
 CONSTRAINT [PK_AuditPlans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[Location] [int] NULL,
	[AttendeeType] [int] NULL,
	[FSU] [int] NULL,
	[ClassTime] [int] NOT NULL,
	[StartedOn] [datetime2](7) NOT NULL,
	[FinishedOn] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[ApprovedOn] [datetime2](7) NOT NULL,
	[ApprovedBy] [int] NULL,
	[TrainingProgramId] [int] NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassUsers]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role] [int] NOT NULL,
	[ClassId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_ClassUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeedBackForms]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedBackForms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Rating] [int] NOT NULL,
	[Comment] [nvarchar](250) NOT NULL,
	[TraineeId] [int] NULL,
	[TrainerId] [int] NULL,
 CONSTRAINT [PK_FeedBackForms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GradeReports]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GradeReports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[GradedOn] [datetime2](7) NOT NULL,
	[Grade] [real] NOT NULL,
	[TraineeId] [int] NOT NULL,
	[LectureId] [int] NOT NULL,
 CONSTRAINT [PK_GradeReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lectures]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lectures](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Duration] [int] NOT NULL,
	[LessonType] [int] NOT NULL,
	[DeliveryType] [int] NOT NULL,
	[UnitId] [int] NULL,
	[OutputStandardId] [int] NULL,
 CONSTRAINT [PK_Lectures] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutputStandards]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutputStandards](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_OutputStandards] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Syllabus]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Syllabus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Version] [real] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[LastModifiedOn] [datetime2](7) NOT NULL,
	[LastModifiedBy] [int] NULL,
	[Level] [int] NOT NULL,
	[AttendeeNumber] [int] NOT NULL,
	[CourseObjectives] [nvarchar](max) NOT NULL,
	[TrainingDeliveryPrincipleId] [int] NULL,
	[QuizCriteria] [real] NOT NULL,
	[AssignmentCriteria] [real] NOT NULL,
	[FinalCriteria] [real] NOT NULL,
	[FinalTheoryCriteria] [real] NOT NULL,
	[FinalPracticalCriteria] [real] NOT NULL,
	[PassingGPA] [real] NOT NULL,
	[isActive] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_Syllabus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SyllabusTrainingProgram]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SyllabusTrainingProgram](
	[SyllabusesId] [int] NOT NULL,
	[TrainingProgramsId] [int] NOT NULL,
 CONSTRAINT [PK_SyllabusTrainingProgram] PRIMARY KEY CLUSTERED 
(
	[SyllabusesId] ASC,
	[TrainingProgramsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SyllabusUnit]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SyllabusUnit](
	[SyllabusesId] [int] NOT NULL,
	[UnitsId] [int] NOT NULL,
 CONSTRAINT [PK_SyllabusUnit] PRIMARY KEY CLUSTERED 
(
	[SyllabusesId] ASC,
	[UnitsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMS]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Time] [datetime2](7) NOT NULL,
	[Reason] [nvarchar](1000) NOT NULL,
	[CheckedBy] [int] NULL,
	[TraineeId] [int] NOT NULL,
 CONSTRAINT [PK_TMS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingDeliveryPrinciple]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingDeliveryPrinciple](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Training] [nvarchar](max) NOT NULL,
	[ReTest] [nvarchar](max) NOT NULL,
	[Marking] [nvarchar](max) NOT NULL,
	[WaiverCriteria] [nvarchar](max) NOT NULL,
	[Others] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TrainingDeliveryPrinciple] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingMaterials]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingMaterials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[LectureId] [int] NOT NULL,
	[EditedOn] [datetime2](7) NOT NULL,
	[EditedBy] [int] NULL,
 CONSTRAINT [PK_TrainingMaterials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingPrograms]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingPrograms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Duration] [int] NOT NULL,
	[LastModify] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[LastModifyBy] [int] NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_TrainingPrograms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Units]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Session] [int] NOT NULL,
 CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/20/2023 9:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](10) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Role] [int] NOT NULL,
	[Level] [int] NULL,
	[Status] [int] NULL,
	[IsMale] [bit] NOT NULL,
	[AvatarURL] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230217030128_initDB', N'7.0.2')
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [FullName], [Email], [Password], [Phone], [DateOfBirth], [Role], [Level], [Status], [IsMale], [AvatarURL]) VALUES (2, N'test', N'test@gmail.com', N'96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e', N'0988123123', CAST(N'2002-01-01T00:00:00.0000000' AS DateTime2), 0, 0, 0, 1, N'test.png')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_AuditDetails_AuditorId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_AuditDetails_AuditorId] ON [dbo].[AuditDetails]
(
	[AuditorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuditDetails_PlanId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_AuditDetails_PlanId] ON [dbo].[AuditDetails]
(
	[PlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuditDetails_TraineeId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_AuditDetails_TraineeId] ON [dbo].[AuditDetails]
(
	[TraineeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuditPlans_PlannedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_AuditPlans_PlannedBy] ON [dbo].[AuditPlans]
(
	[PlannedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuditPlans_SyllabusId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_AuditPlans_SyllabusId] ON [dbo].[AuditPlans]
(
	[SyllabusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Class_ApprovedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_Class_ApprovedBy] ON [dbo].[Class]
(
	[ApprovedBy] ASC
)
WHERE ([ApprovedBy] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Class_CreatedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_Class_CreatedBy] ON [dbo].[Class]
(
	[CreatedBy] ASC
)
WHERE ([CreatedBy] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Class_TrainingProgramId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Class_TrainingProgramId] ON [dbo].[Class]
(
	[TrainingProgramId] ASC
)
WHERE ([TrainingProgramId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClassUsers_ClassId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClassUsers_ClassId] ON [dbo].[ClassUsers]
(
	[ClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClassUsers_UserId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClassUsers_UserId] ON [dbo].[ClassUsers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FeedBackForms_TraineeId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_FeedBackForms_TraineeId] ON [dbo].[FeedBackForms]
(
	[TraineeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FeedBackForms_TrainerId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_FeedBackForms_TrainerId] ON [dbo].[FeedBackForms]
(
	[TrainerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_GradeReports_LectureId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_GradeReports_LectureId] ON [dbo].[GradeReports]
(
	[LectureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_GradeReports_TraineeId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_GradeReports_TraineeId] ON [dbo].[GradeReports]
(
	[TraineeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Lectures_OutputStandardId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Lectures_OutputStandardId] ON [dbo].[Lectures]
(
	[OutputStandardId] ASC
)
WHERE ([OutputStandardId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Lectures_UnitId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_Lectures_UnitId] ON [dbo].[Lectures]
(
	[UnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Syllabus_CreatedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_Syllabus_CreatedBy] ON [dbo].[Syllabus]
(
	[CreatedBy] ASC
)
WHERE ([CreatedBy] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Syllabus_LastModifiedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_Syllabus_LastModifiedBy] ON [dbo].[Syllabus]
(
	[LastModifiedBy] ASC
)
WHERE ([LastModifiedBy] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Syllabus_TrainingDeliveryPrincipleId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Syllabus_TrainingDeliveryPrincipleId] ON [dbo].[Syllabus]
(
	[TrainingDeliveryPrincipleId] ASC
)
WHERE ([TrainingDeliveryPrincipleId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SyllabusTrainingProgram_TrainingProgramsId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_SyllabusTrainingProgram_TrainingProgramsId] ON [dbo].[SyllabusTrainingProgram]
(
	[TrainingProgramsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SyllabusUnit_UnitsId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_SyllabusUnit_UnitsId] ON [dbo].[SyllabusUnit]
(
	[UnitsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TMS_CheckedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_TMS_CheckedBy] ON [dbo].[TMS]
(
	[CheckedBy] ASC
)
WHERE ([CheckedBy] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TMS_TraineeId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_TMS_TraineeId] ON [dbo].[TMS]
(
	[TraineeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TrainingMaterials_EditedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_TrainingMaterials_EditedBy] ON [dbo].[TrainingMaterials]
(
	[EditedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TrainingMaterials_LectureId]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_TrainingMaterials_LectureId] ON [dbo].[TrainingMaterials]
(
	[LectureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TrainingPrograms_CreatedBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_TrainingPrograms_CreatedBy] ON [dbo].[TrainingPrograms]
(
	[CreatedBy] ASC
)
WHERE ([CreatedBy] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TrainingPrograms_LastModifyBy]    Script Date: 2/20/2023 9:55:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_TrainingPrograms_LastModifyBy] ON [dbo].[TrainingPrograms]
(
	[LastModifyBy] ASC
)
WHERE ([LastModifyBy] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attendances]  WITH CHECK ADD  CONSTRAINT [FK_Attendances_ClassUsers_ClassUserId] FOREIGN KEY([ClassUserId])
REFERENCES [dbo].[ClassUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attendances] CHECK CONSTRAINT [FK_Attendances_ClassUsers_ClassUserId]
GO
ALTER TABLE [dbo].[AuditDetails]  WITH CHECK ADD  CONSTRAINT [FK_AuditDetails_AuditPlans_PlanId] FOREIGN KEY([PlanId])
REFERENCES [dbo].[AuditPlans] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditDetails] CHECK CONSTRAINT [FK_AuditDetails_AuditPlans_PlanId]
GO
ALTER TABLE [dbo].[AuditDetails]  WITH CHECK ADD  CONSTRAINT [FK_AuditDetails_Users_AuditorId] FOREIGN KEY([AuditorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AuditDetails] CHECK CONSTRAINT [FK_AuditDetails_Users_AuditorId]
GO
ALTER TABLE [dbo].[AuditDetails]  WITH CHECK ADD  CONSTRAINT [FK_AuditDetails_Users_TraineeId] FOREIGN KEY([TraineeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AuditDetails] CHECK CONSTRAINT [FK_AuditDetails_Users_TraineeId]
GO
ALTER TABLE [dbo].[AuditPlans]  WITH CHECK ADD  CONSTRAINT [FK_AuditPlans_Syllabus_SyllabusId] FOREIGN KEY([SyllabusId])
REFERENCES [dbo].[Syllabus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditPlans] CHECK CONSTRAINT [FK_AuditPlans_Syllabus_SyllabusId]
GO
ALTER TABLE [dbo].[AuditPlans]  WITH CHECK ADD  CONSTRAINT [FK_AuditPlans_Users_PlannedBy] FOREIGN KEY([PlannedBy])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditPlans] CHECK CONSTRAINT [FK_AuditPlans_Users_PlannedBy]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_TrainingPrograms_TrainingProgramId] FOREIGN KEY([TrainingProgramId])
REFERENCES [dbo].[TrainingPrograms] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_TrainingPrograms_TrainingProgramId]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Users_ApprovedBy] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Users_ApprovedBy]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Users_CreatedBy]
GO
ALTER TABLE [dbo].[ClassUsers]  WITH CHECK ADD  CONSTRAINT [FK_ClassUsers_Class_ClassId] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClassUsers] CHECK CONSTRAINT [FK_ClassUsers_Class_ClassId]
GO
ALTER TABLE [dbo].[ClassUsers]  WITH CHECK ADD  CONSTRAINT [FK_ClassUsers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClassUsers] CHECK CONSTRAINT [FK_ClassUsers_Users_UserId]
GO
ALTER TABLE [dbo].[FeedBackForms]  WITH CHECK ADD  CONSTRAINT [FK_FeedBackForms_Users_TraineeId] FOREIGN KEY([TraineeId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FeedBackForms] CHECK CONSTRAINT [FK_FeedBackForms_Users_TraineeId]
GO
ALTER TABLE [dbo].[FeedBackForms]  WITH CHECK ADD  CONSTRAINT [FK_FeedBackForms_Users_TrainerId] FOREIGN KEY([TrainerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[FeedBackForms] CHECK CONSTRAINT [FK_FeedBackForms_Users_TrainerId]
GO
ALTER TABLE [dbo].[GradeReports]  WITH CHECK ADD  CONSTRAINT [FK_GradeReports_Lectures_LectureId] FOREIGN KEY([LectureId])
REFERENCES [dbo].[Lectures] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GradeReports] CHECK CONSTRAINT [FK_GradeReports_Lectures_LectureId]
GO
ALTER TABLE [dbo].[GradeReports]  WITH CHECK ADD  CONSTRAINT [FK_GradeReports_Users_TraineeId] FOREIGN KEY([TraineeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GradeReports] CHECK CONSTRAINT [FK_GradeReports_Users_TraineeId]
GO
ALTER TABLE [dbo].[Lectures]  WITH CHECK ADD  CONSTRAINT [FK_Lectures_OutputStandards_OutputStandardId] FOREIGN KEY([OutputStandardId])
REFERENCES [dbo].[OutputStandards] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Lectures] CHECK CONSTRAINT [FK_Lectures_OutputStandards_OutputStandardId]
GO
ALTER TABLE [dbo].[Lectures]  WITH CHECK ADD  CONSTRAINT [FK_Lectures_Units_UnitId] FOREIGN KEY([UnitId])
REFERENCES [dbo].[Units] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Lectures] CHECK CONSTRAINT [FK_Lectures_Units_UnitId]
GO
ALTER TABLE [dbo].[Syllabus]  WITH CHECK ADD  CONSTRAINT [FK_Syllabus_TrainingDeliveryPrinciple_TrainingDeliveryPrincipleId] FOREIGN KEY([TrainingDeliveryPrincipleId])
REFERENCES [dbo].[TrainingDeliveryPrinciple] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Syllabus] CHECK CONSTRAINT [FK_Syllabus_TrainingDeliveryPrinciple_TrainingDeliveryPrincipleId]
GO
ALTER TABLE [dbo].[Syllabus]  WITH CHECK ADD  CONSTRAINT [FK_Syllabus_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Syllabus] CHECK CONSTRAINT [FK_Syllabus_Users_CreatedBy]
GO
ALTER TABLE [dbo].[Syllabus]  WITH CHECK ADD  CONSTRAINT [FK_Syllabus_Users_LastModifiedBy] FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Syllabus] CHECK CONSTRAINT [FK_Syllabus_Users_LastModifiedBy]
GO
ALTER TABLE [dbo].[SyllabusTrainingProgram]  WITH CHECK ADD  CONSTRAINT [FK_SyllabusTrainingProgram_Syllabus_SyllabusesId] FOREIGN KEY([SyllabusesId])
REFERENCES [dbo].[Syllabus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SyllabusTrainingProgram] CHECK CONSTRAINT [FK_SyllabusTrainingProgram_Syllabus_SyllabusesId]
GO
ALTER TABLE [dbo].[SyllabusTrainingProgram]  WITH CHECK ADD  CONSTRAINT [FK_SyllabusTrainingProgram_TrainingPrograms_TrainingProgramsId] FOREIGN KEY([TrainingProgramsId])
REFERENCES [dbo].[TrainingPrograms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SyllabusTrainingProgram] CHECK CONSTRAINT [FK_SyllabusTrainingProgram_TrainingPrograms_TrainingProgramsId]
GO
ALTER TABLE [dbo].[SyllabusUnit]  WITH CHECK ADD  CONSTRAINT [FK_SyllabusUnit_Syllabus_SyllabusesId] FOREIGN KEY([SyllabusesId])
REFERENCES [dbo].[Syllabus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SyllabusUnit] CHECK CONSTRAINT [FK_SyllabusUnit_Syllabus_SyllabusesId]
GO
ALTER TABLE [dbo].[SyllabusUnit]  WITH CHECK ADD  CONSTRAINT [FK_SyllabusUnit_Units_UnitsId] FOREIGN KEY([UnitsId])
REFERENCES [dbo].[Units] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SyllabusUnit] CHECK CONSTRAINT [FK_SyllabusUnit_Units_UnitsId]
GO
ALTER TABLE [dbo].[TMS]  WITH CHECK ADD  CONSTRAINT [FK_TMS_Users_CheckedBy] FOREIGN KEY([CheckedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TMS] CHECK CONSTRAINT [FK_TMS_Users_CheckedBy]
GO
ALTER TABLE [dbo].[TMS]  WITH CHECK ADD  CONSTRAINT [FK_TMS_Users_TraineeId] FOREIGN KEY([TraineeId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TMS] CHECK CONSTRAINT [FK_TMS_Users_TraineeId]
GO
ALTER TABLE [dbo].[TrainingMaterials]  WITH CHECK ADD  CONSTRAINT [FK_TrainingMaterials_Lectures_LectureId] FOREIGN KEY([LectureId])
REFERENCES [dbo].[Lectures] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrainingMaterials] CHECK CONSTRAINT [FK_TrainingMaterials_Lectures_LectureId]
GO
ALTER TABLE [dbo].[TrainingMaterials]  WITH CHECK ADD  CONSTRAINT [FK_TrainingMaterials_Users_EditedBy] FOREIGN KEY([EditedBy])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrainingMaterials] CHECK CONSTRAINT [FK_TrainingMaterials_Users_EditedBy]
GO
ALTER TABLE [dbo].[TrainingPrograms]  WITH CHECK ADD  CONSTRAINT [FK_TrainingPrograms_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TrainingPrograms] CHECK CONSTRAINT [FK_TrainingPrograms_Users_CreatedBy]
GO
ALTER TABLE [dbo].[TrainingPrograms]  WITH CHECK ADD  CONSTRAINT [FK_TrainingPrograms_Users_LastModifyBy] FOREIGN KEY([LastModifyBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TrainingPrograms] CHECK CONSTRAINT [FK_TrainingPrograms_Users_LastModifyBy]
GO
USE [master]
GO
ALTER DATABASE [FATMSDB] SET  READ_WRITE 
GO
