CREATE PROCEDURE [dbo].[_spRemoveWish]
	@Id NVARCHAR(50)
AS
begin
	DELETE FROM [dbo].[WishLists] WHERE Id = @Id;
end