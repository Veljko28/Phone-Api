using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Implementation
{
	public class PhoneRepository : IPhoneRepository
	{
		public async Task<bool> AddPhoneAsync(PhoneRequest phone)
		{
			throw new NotImplementedException();
		}

		public async  Task<PhoneResponse> GetPhoneInfoByIdAsync(string Id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<PhoneResponse>> SearchPhonesAsync(string search)
		{
			throw new NotImplementedException();
		}
	}
}
