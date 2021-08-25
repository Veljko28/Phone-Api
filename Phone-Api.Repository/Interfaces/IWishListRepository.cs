using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IWishListRepository
	{
		Task<GenericResponse> AddToWishListAsync(WishListRequest model);
		Task<GenericResponse> RemoveFromWishListAsync(string UserId, string PhoneId);
		Task<IEnumerable<string>> GetUserWishesAsync(UserWishListRequest model);
		Task<List<string>> FindUserWishListAsync(PageInWishListRequest model);
		Task<int> GetPhoneFavoritesAsync(string phoneId);
	}
}
