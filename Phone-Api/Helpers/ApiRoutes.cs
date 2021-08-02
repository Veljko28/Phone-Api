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

			public const string Featured = root + version + type + "/featured";

			public const string Latest = root + version + type + "/latest";

			public const string GetSellerPhones = root + version + type + "/seller" + "/{sellerId}";

			public const string GetImages = root + version + type + "/images" + "/{phoneId}";

			public const string GetPage = root + version + type + "/page" + "/{pageId}";


		}
		public static class UserRoutes
		{
			public const string type = "/users";

			public const string Register = root + version + type + "/register";

			public const string Login = root + version + type + "/login";

			public const string ChangePassword = root + version + type + "/changepassword/{userId}";
			
			public const string Refresh = root + version + "/token" + "/refresh"; 

			public const string GetUserById = root + version + type + "/{userId}";

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

			public const string UserImageUpload = root + version + type + "/user" +"/image";

			public const string PhoneBidImageUpload = root + version + type + "/phone" + "/image";

			public const string PhoneDisplay = root + version + type + "/phone" + "/display";

			public const string Contact = root + version + type + "/contact";

			public const string Subscribe = root + version + type + "/subscribe";

			public const string Search = root + version + type + "/search/{term}";

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

			public const string UserBids = root + version + type + "/user" + "/{userId}";

			public const string AddHistory = root + version + type +  "/addhistory";

			public const string BidHistories = root + version + type +  "/histories/{bid_Id}";
		}

	}
}
