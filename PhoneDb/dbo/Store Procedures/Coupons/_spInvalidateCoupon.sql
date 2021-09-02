CREATE PROCEDURE [dbo].[_spInvalidateCoupon]
	@Id int
AS
begin
	UPDATE [dbo].[UserCoupons] SET Valid = 0 WHERE Id = @Id;
end