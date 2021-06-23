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
		//Task<IEnumerable<PhoneResponse>> GetAllPhonesAsync();
		Task<PhoneResponse> GetPhoneInfoByIdAsync(string Id);
		Task<IEnumerable<PhoneResponse>> SearchPhonesAsync(string search);
		Task<bool> AddPhoneAsync(PhoneRequest phone);

	}
}
