CREATE TABLE [dbo].[Lists] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,	
    [BoardId]     UNIQUEIDENTIFIER NULL,
    [Name]     NVARCHAR (50)    NULL,
    [MaxItems] INT         NULL, 
    [Position] INT NULL
);

