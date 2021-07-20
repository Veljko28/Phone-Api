CREATE PROCEDURE [dbo].[_spGetUserById]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[Users] WHERE Id = @Id;
end