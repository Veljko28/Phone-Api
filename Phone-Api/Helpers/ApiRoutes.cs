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
			public const string type = root + version + "/phones";

			public const string Add = type + "/add/{userId}";

			public const string GetById = type + "/{phoneId}";

			public const string Featured = type + "/featured";

			public const string Latest = type + "/latest";

			public const string GetSellerPhones = type + "/seller" + "/{sellerId}/{pageNum}";

			public const string GetImages = type + "/images" + "/{phoneId}";

			public const string GetPage = type + "/page" + "/{pageId}";

			public const string Delete = type + "/delete" + "/{phoneId}";

			public const string Reviews = type + "/reviews" + "/{phoneId}";

			public const string Edit = type + "/edit";
			
			public const string ChangeStatus = type + "/status";

		}

		public static class UserRoutes
		{
			public const string type = root + version + "/users";

			public const string Register = type + "/register";

			public const string Login = type + "/login";

			public const string ChangePassword = type + "/changepassword/{userId}";
			
			public const string Refresh = root + version + "/token" + "/refresh"; 

			public const string GetUserById = type + "/{userId}";

			public const string EditUserProfile = type + "/edit/{userId}";

			public const string GetUserNameById = type + "/username/{userId}";


		}

		public static class WishlistRoutes
		{
			public const string type = root + version + "/wishlist";

			public const string AddToWishList = type + "/add";

			public const string RemoveFromWishList = type + "/remove";

			public const string GetUserWishes = type + "/get";

		}

		public static class GenericRoutes
		{
			public const string type = root + version + "/generic";

			public const string UserImageUpload = type + "/user" +"/image";

			public const string PhoneBidImageUpload = type + "/phone" + "/image";

			public const string PhoneDisplay = type + "/phone" + "/display";

			public const string Contact = type + "/contact";

			public const string Subscribe = type + "/subscribe";

			public const string Search = type + "/search/{term}";

			public const string AddReview = type + "/review";

		}

		public static class PurchaseRoutes
		{
			public const string type = root + version + "/purchase";

			public const string AddPhoneToPurchase = type + "/phonetopurchase/{purchaseId}/{phoneId}";

			public const string AddPurchase = type + "/addpurchase";

			public const string GetPurchasePhones = type + "/getphones/{purchaseId}";
		}


		public static class BidRoutes
		{
			public const string type = root + version + "/bids";

			public const string AddBid = type + "/add/{userId}";

			public const string BidById = type + "/{bid_Id}";

			public const string UserBids = type + "/user" + "/{userId}";

			public const string AddHistory = type +  "/addhistory";

			public const string BidHistories = type +  "/histories/{bid_Id}";

			public const string GetPage = type + "/page" + "/{pageId}";

			public const string GetImages = type + "/images" + "/{bid_Id}";

			public const string Delete = type + "/delete" + "/{bid_Id}";


		}

		public static class NotificationsRoutes
		{
			public const string type = root + version + "/notifications";

			public const string AddNotification = type + "/add";

			public const string RemoveNotification = type + "/remove/{notificationId}";

			public const string GetUserNotifications = type + "/{userId}";
		}

	}
}
