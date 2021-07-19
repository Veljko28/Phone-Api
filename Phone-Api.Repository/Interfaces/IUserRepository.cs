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
		Task<GenericResponse> RegisterAsync(UserRequest userRequest);

		Task<UserModel> LoginAsync(LoginRequest loginRequest);
		Task<GenericResponse> ChangePasswordAsync(string userId, ChangePasswordRequest change);
	}
}
