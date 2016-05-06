CREATE TABLE [dbo].[Cards] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [Title]       NVARCHAR (MAX)   NULL,
    [ListId]      UNIQUEIDENTIFIER NULL,
    [Position]    INT              NULL
);

