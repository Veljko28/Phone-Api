CREATE PROCEDURE [dbo].[_spBidImageUpload]
	@Id NVARCHAR(50),
	@Image NVARCHAR(256)
AS
begin
  INSERT INTO  [dbo].[BidImages] (Bid_Id, ImagePath) VALUES (@Id, @Image);
end