USE [FATMSDB]
GO
INSERT [dbo].[Role] ([Id], [Name], [SyllabusPermission], [TrainingProgramPermission], [ClassPermission], [LearningMaterialPermission], [UserPermission]) VALUES (1, N'Super Admin', 5, 5, 5, 5, 5)
GO
INSERT [dbo].[Role] ([Id], [Name], [SyllabusPermission], [TrainingProgramPermission], [ClassPermission], [LearningMaterialPermission], [UserPermission]) VALUES (2, N'Class Admin', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Role] ([Id], [Name], [SyllabusPermission], [TrainingProgramPermission], [ClassPermission], [LearningMaterialPermission], [UserPermission]) VALUES (3, N'Trainer', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Role] ([Id], [Name], [SyllabusPermission], [TrainingProgramPermission], [ClassPermission], [LearningMaterialPermission], [UserPermission]) VALUES (4, N'Auditor', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Role] ([Id], [Name], [SyllabusPermission], [TrainingProgramPermission], [ClassPermission], [LearningMaterialPermission], [UserPermission]) VALUES (5, N'Trainee', NULL, NULL, NULL, NULL, NULL)
GO
