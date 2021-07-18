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

			public const string Add = root + version + type + "/add";

			public const string GetById = root + version + type + "/{phoneId}";

		}
		public static class UserRoutes
		{
			public const string type = "/users";

			public const string Register = root + version + type + "/register";

			public const string Login = root + version + type + "/login";

		}
	}
}
