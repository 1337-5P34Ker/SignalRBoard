CREATE TABLE [dbo].[Activities] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [CardId]     UNIQUEIDENTIFIER NULL,
    [Action]     NVARCHAR (MAX)   NULL,
    [ModifiedAt] DATETIME         NULL
);

