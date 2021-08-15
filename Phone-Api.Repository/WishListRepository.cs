using Dapper;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository
{
	public class WishListRepository : IWishListRepository
	{
		private readonly IConfiguration _configuration;

		public WishListRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<GenericResponse> AddToWishListAsync(WishListRequest model)
		{
			WishListModel wish = new WishListModel
			{
				Id = Guid.NewGuid().ToString(),
				UserId = model.UserId,
				PhoneId = model.PhoneId,
				Type = model.Type
			};

			string sql = "exec [_spAddToWishList] @Id, @UserId, @PhoneId, @Type";
			return await DatabaseOperations.GenericExecute(sql, wish, _configuration, "Failed to insert the item to the wish list");
		}

		public async Task<IEnumerable<string>> GetUserWishesAsync(UserWishListRequest model)
		{
			string sql = "exec [_spGetUserWishes] @UserId, @Type";

			return await DatabaseOperations.GenericQueryList<dynamic,string>(sql, new { model.UserId, model.Type }, _configuration);
		}

		public async Task<GenericResponse> RemoveFromWishListAsync(string Id)
		{
			string sql = "exec [_spRemoveWish] @Id";

			return await DatabaseOperations.GenericExecute(sql, new { Id }, _configuration, "Failed to remove the item from the wish list");

		}
	}
}
