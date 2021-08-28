CREATE PROCEDURE [dbo].[_spGetPurchasedPhones]
	@UserId NVARCHAR(50),
	@Page int
AS
begin
	 SELECT TOP 8 PhoneId FROM (
            SELECT ROW_NUMBER() OVER(ORDER BY Id) AS RoNum
                  , *
            FROM [dbo].[PhonePurchases]
    ) AS tbl 
    WHERE (@Page-1)*8 < RoNum AND tbl.[BuyerId] = @UserId
    ORDER BY tbl.Id
end