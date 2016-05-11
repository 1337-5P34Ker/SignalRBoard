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
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'03576f48-f9fa-4dc5-b6fe-47f5b30a0829', N'AAA', N'AAA', N'fb9344df-0d30-4183-bd15-d3c2e4afe978', 0)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'b642ec22-8102-43a3-bb71-454dab09ed48', N'Und ich bin die Beschreibung', N'Ich bin der Titel der Karte (Stack 2) a', N'fb9344df-0d30-4183-bd15-d3c2e4afe9aa', 0)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'c9e984e6-2592-42a8-990e-42543f55aff1', N'Bla bla bla', N'Ich bin der Titel der Karte (Stack 3)', N'fb9344df-0d30-4183-bd15-d3c2e4afe9bb', 0)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'fb6066a9-fac9-4ab9-bb75-c5cf14435efc', N'CCC', N'CCC', N'fb9344df-0d30-4183-bd15-d3c2e4afe978', 2)
GO
INSERT [dbo].[Cards] ([Id], [Description], [Title], [ListId], [Position]) VALUES (N'a9e71f39-78be-4cc2-8cc8-782e52675b8f', N'BBB', N'BBB', N'fb9344df-0d30-4183-bd15-d3c2e4afe978', 1)
GO
INSERT [dbo].[Lists] ([Id], [BoardId], [Name], [MaxItems], [Position]) VALUES (N'fb9344df-0d30-4183-bd15-d3c2e4afe9aa', N'c7b846c4-589f-473f-b8a8-ec260c1efe93', N'Stack 2', 3, 2)
GO
INSERT [dbo].[Lists] ([Id], [BoardId], [Name], [MaxItems], [Position]) VALUES (N'fb9344df-0d30-4183-bd15-d3c2e4afe9bb', N'c7b846c4-589f-473f-b8a8-ec260c1efe93', N'Stack 3', 3, 3)
GO
INSERT [dbo].[Lists] ([Id], [BoardId], [Name], [MaxItems], [Position]) VALUES (N'fb9344df-0d30-4183-bd15-d3c2e4afe978', N'c7b846c4-589f-473f-b8a8-ec260c1efe93', N'Stack 1', 3, 1)
GO