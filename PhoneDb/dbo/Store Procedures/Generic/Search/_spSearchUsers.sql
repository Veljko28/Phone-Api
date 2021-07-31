CREATE PROCEDURE [dbo].[_spSearchUsers]
	@Term NVARCHAR(256)
AS
begin
	SELECT * FROM [dbo].[Users] WHERE [UserName] LIKE @Term OR [Description] LIKE @Term OR [Email] LIKE @Term;
end