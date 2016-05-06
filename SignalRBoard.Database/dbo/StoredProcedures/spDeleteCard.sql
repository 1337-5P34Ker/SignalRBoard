CREATE PROCEDURE [dbo].[spDeleteCard] @Id UNIQUEIDENTIFIER = NULL 
AS 
  BEGIN 
      DELETE FROM [dbo].[cards] 
      WHERE  id = @Id 
  END 