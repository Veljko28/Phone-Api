using Phone_Api.Models.Responses;
using Phone_Api.Models.ReviewModels.Requests;
using Phone_Api.Models.ReviewModels;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Phone_Api.Repository.Helpers;
using Microsoft.Extensions.Configuration;

namespace Phone_Api.Repository
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly IConfiguration _configuration;
		public ReviewRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<GenericResponse> AddReviewAsync(ReviewModelRequest req)
		{
			ReviewModel model = new ReviewModel
			{
				Id = Guid.NewGuid().ToString(),
				Rating = req.Rating,
				BuyerId = req.BuyerId,
				SellerId = req.SellerId,
				PhoneId = req.PhoneId,
				DateCreated = req.DateCreated,
				Message = req.Message
			};

			string sql = "exec [_spAddReview] @Id, @Rating, @BuyerId, @SellerId, @PhoneId, @DateCreated, @Message";

			return await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to add the review");
		}

		public async Task<bool> CheckReviewedAsync(ReviewedCheckRequest request)
		{
			string sql = "exec [_spFindUserReview] @BuyerId, @PhoneId";

			var model = await DatabaseOperations.GenericQuerySingle<dynamic, ReviewModel>(sql, request, _configuration);

			return model != null;
		}

		public async Task<IEnumerable<ReviewModel>> GetUserReviewsAsync(string userId)
		{
			string sql = "exec [_spGetUserReviews] @SellerId";

			return await DatabaseOperations.GenericQueryList<dynamic, ReviewModel>(sql, new { SellerId = userId }, _configuration);
		}

	}
}
