using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Phone_Api.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Phone_Api.Repository.Helpers
{
	public static class JwtValidation
	{
		private static bool isJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
		{
			return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
				jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
		}

		public static ClaimsPrincipal getPrincipalFromToken(string token, IConfiguration configuration)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var jwtSettings = new JwtSettings();
			configuration.Bind(nameof(JwtSettings), jwtSettings);

			try
			{

				var tokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
					RequireExpirationTime = false,
					ValidateLifetime = false
				};

				var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
				if (!isJwtWithValidSecurityAlgorithm(validatedToken))
				{
					return null;
				}

				return principal;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}

		}

	}
}
