CREATE PROCEDURE [dbo].[spGetCards]
	@ListId uniqueidentifier = NULL
AS
	SELECT [Id]
      ,[Description]
      ,[Title]
      ,[ListId]
      ,[Position]
  FROM [SignalRBoard].[dbo].[Cards] 
  WHERE
  (@ListId IS NULL OR [ListId] = @ListId)
  ORDER BY [Position]

