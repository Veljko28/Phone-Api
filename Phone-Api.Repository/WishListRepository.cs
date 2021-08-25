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

		public async Task<List<string>> FindUserWishListAsync(PageInWishListRequest model)
		{
			string sql = "exec [_spFindUserWishList] @UserId, @PhoneId, @Type";
			List<string> phoneIds = new List<string>();

			foreach (PhoneModel phone in model.List)
			{

				bool isInWishList = await DatabaseOperations.GenericQuerySingle
					<dynamic, WishListModel>(sql, new { phoneId = phone.Id, model.Type, model.UserId }, _configuration) != null;

				if (isInWishList)
				{
					phoneIds.Add(phone.Id);
				}
			}

			return phoneIds;
		}

		public async Task<int> GetPhoneFavoritesAsync(string phoneId)
		{
			string sql = "SELECT COUNT(*) FROM [dbo].[WishLists] WHERE PhoneId = '" + phoneId + "'";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int numOfFavorites = await db.ExecuteScalarAsync<int>(sql);

				return numOfFavorites;
			}
		}

		public async Task<IEnumerable<string>> GetUserWishesAsync(UserWishListRequest model)
		{
			string sql = "exec [_spGetUserWishes] @UserId, @Type";

			return await DatabaseOperations.GenericQueryList<dynamic, string>(sql, new { model.UserId, model.Type }, _configuration);
		}

		public async Task<GenericResponse> RemoveFromWishListAsync(string UserId, string PhoneId)
		{
			string sql = "exec [_spRemoveWish] @UserId, @PhoneId";

			return await DatabaseOperations.GenericExecute(sql, new {UserId, PhoneId}, _configuration, "Failed to remove the item from the wish list");
		}
	}
}
