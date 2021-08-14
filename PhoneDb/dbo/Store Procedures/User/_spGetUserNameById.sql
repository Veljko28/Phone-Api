CREATE PROCEDURE [dbo].[_spGetUserNameById]
	@Id NVARCHAR(50)
AS
begin
	SELECT UserName FROM [dbo].[Users] WHERE Id = @Id;
end	