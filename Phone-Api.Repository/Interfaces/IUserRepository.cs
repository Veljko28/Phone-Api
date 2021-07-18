using Phone_Api.Models;
using Phone_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IUserRepository
	{
		Task<bool> RegisterAsync(UserRequest userRequest);

		Task<UserModel> LoginAsync(LoginRequest loginRequest);
	}
}
