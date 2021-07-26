CREATE PROCEDURE [dbo].[_spAddSubscribeEmail]
	@Id NVARCHAR(50),
	@Email NVARCHAR(256)
AS
begin
	INSERT INTO [dbo].[SubscribeEmails] (Id, Email) VALUES (@Id, @Email);
end