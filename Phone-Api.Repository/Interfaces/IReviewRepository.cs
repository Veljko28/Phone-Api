using Phone_Api.Models;
using Phone_Api.Models.Responses;
using Phone_Api.Models.ReviewModels.Requests;
using Phone_Api.Models.ReviewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IReviewRepository
	{
		Task<GenericResponse> AddReviewAsync(ReviewModelRequest req);
		Task<bool> CheckReviewedAsync(ReviewedCheckRequest request);
	}
}
