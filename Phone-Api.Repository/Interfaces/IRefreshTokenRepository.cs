using Phone_Api.Models;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IRefreshTokenRepository
	{
		Task<RefreshToken> FindTokenAsync(string Token);

		Task<bool> AddTokenAsync(RefreshToken refreshToken);

		Task<bool> UpdateTokenAsync(RefreshToken refreshToken);
	}
}
