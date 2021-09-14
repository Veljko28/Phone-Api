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

		Task<PhoneModel> GetPhoneByIdAsync(string Id);
		Task<PhoneModel> AddPhoneAsync(PhoneRequest phoneRequest, string userId);
		Task<IEnumerable<PhoneModel>> GetPhonesAsync();
		Task<IEnumerable<PhoneModel>> GetLastestPhonesAsync();
		Task<IEnumerable<PhoneModel>> GetFeaturedPhonesAsync(string phoneId);
		Task<IEnumerable<PhoneModel>> GetSellerPhonesByIdAsync(string sellerId, int pageNum);
		Task<IEnumerable<string>> GetPhoneImagesAsync(string phoneId);
		Task<GenericResponse> DeletePhoneAsync(string phoneId);
		Task<GenericResponse> EditPhoneAsync(PhoneModel model);
		Task<GenericResponse> ChangeStatusAsync(ChangePhoneStatusRequest request);
		Task<int> GetNumOfPagesAsync(string sellerId = null);
		Task<int> GetNumOfUserPhonesAsync(string userId);

	}

}
