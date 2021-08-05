CREATE PROCEDURE [dbo].[_spDeletePhone]
	@Id nvarchar(50)
AS
begin
	DELETE FROM [dbo].[Phones] WHERE Id = @Id;
end