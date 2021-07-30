using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly IConfiguration _configuration;

		public RefreshTokenRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<bool> AddTokenAsync(RefreshToken refreshToken)
		{
			string sql = "exec [AddRefreshToken] @Token, @JwtId, @CreatedDate, @Expires, @Used, @Invalidated, @UserId";

			return (await DatabaseOperations.GenericExecute(sql, refreshToken, _configuration, "Failed to add the refresh token")).Success;

		}

		public async Task<RefreshToken> FindTokenAsync(string Token)
		{
			string sql = "exec [FindRefreshToken] @Token";

			return await DatabaseOperations.GenericQuerySingle<dynamic, RefreshToken>(sql, new { Token }, _configuration);
		}

		public async Task<bool> UpdateTokenAsync(RefreshToken refreshToken)
		{
			string sql = "exec [UpdateRefeshToken] @Token, @JwtId, @CreatedDate, @Expires, @Used, @Invalidated, @UserId";

			return (await DatabaseOperations.GenericExecute(sql, refreshToken, _configuration, "Failed to update the refresh token")).Success;
		
		}
	}
}
