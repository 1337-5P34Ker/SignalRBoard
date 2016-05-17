/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE FROM [dbo].[Boards]
GO

DELETE FROM [dbo].[Lists]
GO

DELETE FROM [dbo].[Cards]
GO

INSERT [dbo].[Boards] ([Id], [Name]) VALUES (N'c7b846c4-589f-473f-b8a8-ec260c1efe93', N'Board 1')
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'56c21b2b-7827-4cc2-9ca9-5ee83c32f09e', N'Beschreibung von ToDo 1', N'ToDo 1', N'fb9344df-0d30-4183-bd15-d3c2e4afe978', 0)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'b642ec22-8102-43a3-bb71-454dab09ed48', N'Beschreibung von Done 2', N'Done 1', N'fb9344df-0d30-4183-bd15-d3c2e4afe9bb', 0)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'1ce86898-e837-41bb-a423-e1f9954d5fb9', N'Beschreibung von ToDo 2', N'ToDo 2', N'fb9344df-0d30-4183-bd15-d3c2e4afe978', 1)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'715a0eb8-6e7d-46ef-b5a0-183686562ab5', N'Beschreibung von In Progress 1', N'In Progress 1', N'fb9344df-0d30-4183-bd15-d3c2e4afe9aa', 0)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'0dd9c4bd-7e8f-450f-803a-60d5a413bec9', N'Beschreibung von Done 2', N'Done 2', N'fb9344df-0d30-4183-bd15-d3c2e4afe9bb', 1)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'c77b45f7-2014-4c8f-9039-802c7f8467a8', N'Beschreibung von In Progress 2', N'In Progress 2', N'fb9344df-0d30-4183-bd15-d3c2e4afe9aa', 1)
GO
INSERT [dbo].[Lists] ([Id], [BoardId], [Name], [MaxItems], [Position]) VALUES (N'fb9344df-0d30-4183-bd15-d3c2e4afe9aa', N'c7b846c4-589f-473f-b8a8-ec260c1efe93', N'In Progress', 3, 1)
GO
INSERT [dbo].[Lists] ([Id], [BoardId], [Name], [MaxItems], [Position]) VALUES (N'fb9344df-0d30-4183-bd15-d3c2e4afe9bb', N'c7b846c4-589f-473f-b8a8-ec260c1efe93', N'Done', 3, 2)
GO
INSERT [dbo].[Lists] ([Id], [BoardId], [Name], [MaxItems], [Position]) VALUES (N'fb9344df-0d30-4183-bd15-d3c2e4afe978', N'c7b846c4-589f-473f-b8a8-ec260c1efe93', N'ToDo', 3, 0)
GO