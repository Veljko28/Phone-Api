CREATE PROCEDURE [dbo].[_spSearchPhones]
	@Term NVARCHAR(256)
AS
begin
	SELECT * FROM [dbo].[Phones] WHERE [Name] LIKE @Term OR [Description] LIKE @Term OR Brand LIKE @Term;
end