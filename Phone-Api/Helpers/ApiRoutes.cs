using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Helpers
{
	public static class ApiRoutes
	{
		public const string root = "/api";
		public const string version = "/v1";
		public static class PhoneRoutes
		{
			public const string type = "/phones";

			public const string Add = root + version + type + "/add/{userId}";

			public const string GetById = root + version + type + "/{phoneId}";

			public const string GetSellerPhones = root + version + type + "/seller" + "/{sellerId}";

		}
		public static class UserRoutes
		{
			public const string type = "/users";

			public const string Register = root + version + type + "/register";

			public const string Login = root + version + type + "/login";

			public const string ChangePassword = root + version + type + "/changepassword/{userId}";

		}

		public static class WishlistRoutes
		{
			public const string type = "/wishlist";

			public const string AddToWishList = root + version + type + "/add";

			public const string RemoveFromWishList = root + version + type + "/remove/{wishId}";

			public const string GetUserWishes = root + version + type + "/get/{userId}";

		}

		public static class GenericRoutes
		{
			public const string type = "/generic";

			public const string ImageUpload = root + version + type + "/uploadimage";

			public const string Contact = root + version + type + "/contact";

			public const string Subscribe = root + version + type + "/subscribe";

		}

		public static class PurchaseRoutes
		{
			public const string type = "/purchase";

			public const string AddPhoneToPurchase = root + version + type + "/phonetopurchase/{purchaseId}/{phoneId}";

			public const string AddPurchase = root + version + type + "/addpurchase";

			public const string GetPurchasePhones = root + version + type + "/getphones/{purchaseId}";
		}


		public static class BidRoutes
		{
			public const string type = "/bid";

			public const string AddBid = root + version + type + "/add";

			public const string BidById = root + version + type + "/{bid_Id}";

			public const string AddHistory = root + version + type +  "/addhistory";

			public const string BidHistories = root + version + type +  "/histories/{bid_Id}";
		}

	}
}
