CREATE PROCEDURE [dbo].[_spAddUserCoupon]
	@UserId NVARCHAR(50),
	@Coupon NVARCHAR(15),
	@Amount NVARCHAR(5)
AS
begin
	INSERT INTO [dbo].[UserCoupons] (UserId, Coupon, Amount) VALUES (@UserId, @Coupon, @Amount);
end