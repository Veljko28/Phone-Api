using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Helpers
{
	public static class ApiRoutes
	{
		public static class Phones
		{
			public const string Add = "api/phone/add";

			public const string Id = "api/phone/{phoneId}";

			public const string Search = "api/phone/search/{keyword}";
		}

		public static class Users
		{
			public const string Register = "api/user/register";

			public const string Login = "api/user/login";

		}
	}
}
