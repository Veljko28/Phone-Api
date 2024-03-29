﻿using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Models.ReviewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IUserRepository
	{
		Task<int> GetLoyalityPointsAsync(string userId);
		Task<bool> UpdatePhonesSoldAsync(string userId);
		Task<UserModel> GetUserByIdAsync(string Id);
		Task<UserModel> GetUserByEmailAsync(string email);
		Task<string> GetUserIdByNameAsync(string userName);
		Task<string> GetEmailByIdAsync(string userId);
		Task<string> GetUserNameByIdAsync(string userId);
		Task<string> RegisterAsync(UserRequest userRequest);
		Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
		Task<TokenResponse> RefreshTokenAsync(string token, string refreshToken);
		Task<GenericResponse> UpdateRatingAsync(double rating, string userId);
		Task<GenericResponse> AddLoyalityPointsAsync(LoyalityPointsRequest request);
		Task<GenericResponse> RemoveLoyalityPointsAsync(LoyalityPointsRequest request);
		Task<GenericResponse> EditUserProfileAsync(string userId, EditProfileModel model);
		Task<GenericResponse> ChangePasswordAsync(string userId, ChangePasswordRequest change);
		Task<GenericResponse> AddUserCouponAsync(CouponModel model);
		Task<IEnumerable<ReviewModel>> GetUserReviewsAsync(string userId);

	}
}
