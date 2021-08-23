using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IUserRepository
	{
		Task<UserModel> GetUserByIdAsync(string Id);
		Task<UserModel> GetUserByEmailAsync(string email);
		Task<string> GetUserNameByIdAsync(string userId);
		Task<string> GetEmailByIdAsync(string userId);
		Task<string> RegisterAsync(UserRequest userRequest);
		Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
		Task<TokenResponse> RefreshTokenAsync(string token, string refreshToken);
		Task<GenericResponse> EditUserProfileAsync(string userId, EditProfileModel model);
		Task<GenericResponse> ChangePasswordAsync(string userId, ChangePasswordRequest change);
	}
}
