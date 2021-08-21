using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IPhoneRepository
	{

		Task<PhoneModel> AddPhoneAsync(PhoneRequest phoneRequest, string userId);
		Task<PhoneModel> GetPhoneByIdAsync(string Id);
		Task<IEnumerable<PhoneModel>> GetSellerPhonesByIdAsync(string sellerId, int pageNum);
		Task<IEnumerable<string>> GetPhoneImagesAsync(string phoneId);
		Task<IEnumerable<PhoneModel>> GetLastestPhonesAsync();
		Task<IEnumerable<PhoneModel>> GetFeaturedPhonesAsync(string phoneId);
		Task<IEnumerable<PhoneModel>> GetPhonePageAsync(string page);
		Task<GenericResponse> DeletePhoneAsync(string phoneId);
		Task<GenericResponse> EditPhoneAsync(PhoneModel model);
		Task<IEnumerable<ReviewModel>> GetPhoneReviewsById(string Id);
		Task<GenericResponse> ChangeStatusAsync(ChangePhoneStatusRequest request);
		Task<int> GetNumOfPagesAsync();

	}

}
