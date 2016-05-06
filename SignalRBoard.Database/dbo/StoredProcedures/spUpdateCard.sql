CREATE PROCEDURE [dbo].[SpUpdateCard] @ListId      UNIQUEIDENTIFIER = NULL, 
                                      @Id          UNIQUEIDENTIFIER = NULL, 
                                      @Description NVARCHAR(max) = NULL, 
                                      @Title       NVARCHAR(max) = NULL, 
                                      @Position    INT = NULL 
AS 
    DECLARE @NewCardId AS UNIQUEIDENTIFIER; 

    IF EXISTS (SELECT * 
               FROM   [dbo].[cards] 
               WHERE  id = @Id) 
      BEGIN 
          IF( @ListId IS NULL ) 
            BEGIN 
                SET @ListId = (SELECT listid 
                               FROM   [dbo].[cards] 
                               WHERE  id = @Id); 
            END 

          IF ( @Position IS NULL ) -- Karte ans Ende  
            BEGIN 
                SET @Position = (SELECT Count(*) 
                                 FROM   [dbo].[cards] 
                                 WHERE  listid = @ListId); 
            END 

          UPDATE [SignalRBoard].[dbo].[cards] 
          SET    [description] = @Description, 
                 [title] = @Title, 
                 [listid] = @ListId, 
                 [position] = @Position 
          WHERE  [id] = @Id 

          SET @NewCardId = @Id 
      END 
    ELSE 
      BEGIN 
          SET @NewCardId = Newid(); 
          SET @Position = (SELECT Count(*) 
                           FROM   [dbo].[cards] 
                           WHERE  listid = @ListId); 

          INSERT INTO [dbo].[cards] 
                      ([id], 
                       [description], 
                       [title], 
                       [listid], 
                       [position]) 
          VALUES      ( @NewCardId, 
                        @Description, 
                        @Title, 
                        @ListId, 
                        @Position) 
      END 

    SELECT [id], 
           [description], 
           [title], 
           [listid], 
           [position] 
    FROM   [dbo].[cards] 
    WHERE  id = @NewCardId 