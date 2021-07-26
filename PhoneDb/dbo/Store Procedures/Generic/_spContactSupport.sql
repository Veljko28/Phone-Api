CREATE PROCEDURE [dbo].[_spContactSupport]
	@Id NVARCHAR(50),
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @Subject NVARCHAR(100), 
    @Message NVARCHAR(MAX)
AS
begin
	INSERT INTO [dbo].[ContactSupport] (Id, [Name], [Email], [Subject], [Message]) VALUES (@Id, @Name, @Email, @Subject, @Message);
end