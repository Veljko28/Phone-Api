using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Phone_Api.Repository
{
	internal static class PasswordHashing
	{
		public static string HashPassword(string password)
		{

			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
			byte[] hash = pbkdf2.GetBytes(20);


			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);
			string passwordHash = Convert.ToBase64String(hashBytes);

			return passwordHash;
		}

		public static bool ComparePasswords(string dbPassword, string inputPassword)
		{
			// DEHASH THE PASSWORD STORED IN THE DATABASE
			byte[] hashBytes = Convert.FromBase64String(dbPassword);
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);

			var passHash = new Rfc2898DeriveBytes(inputPassword, salt, 100000);
			byte[] hash = passHash.GetBytes(20);

			// COMPARE THE RESULTS 

			for (int i = 0; i < 20; i++)
				if (hashBytes[i + 16] != hash[i])
					return false;

			return true;
		}
	}
}
