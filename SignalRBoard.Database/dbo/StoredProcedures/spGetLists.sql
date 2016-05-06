CREATE PROCEDURE [dbo].[spGetLists]
	@BoardId uniqueidentifier = NULL
AS
	SELECT [Id]
      ,[Name]
      ,[MaxItems]
	  ,[Position]
  FROM [SignalRBoard].[dbo].[Lists] 
  WHERE
  (@BoardId IS NULL OR [BoardId] = @BoardId)
  ORDER BY [Position]
 