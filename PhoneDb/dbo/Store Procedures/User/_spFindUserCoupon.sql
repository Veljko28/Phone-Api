CREATE PROCEDURE [dbo].[_spFindUserCoupon]
	@Coupon NVARCHAR(15)
AS
begin
	SELECT * FROM [dbo].[UserCoupons] WHERE Coupon = @Coupon;
end
