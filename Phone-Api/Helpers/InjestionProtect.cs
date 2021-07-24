using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Helpers
{
	public static class InjestionProtect
	{
		public static string StringSqlRemove(string term)
		{
			char[] arr = term.Where(c => (char.IsLetterOrDigit(c) ||
							char.IsWhiteSpace(c) ||
							c == '-')).ToArray();

			return new string(arr);
		}
	}
}
