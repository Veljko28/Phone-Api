using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
using Phone_Api.Repository.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Implementation
{
	public class PhoneRepository : IPhoneRepository
	{
		private readonly AppDbContext _context;

		public PhoneRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task<bool> AddPhoneAsync(PhoneRequest phone)
		{
			PhoneResponse phoneResponse = new PhoneResponse
			{
				Id = Guid.NewGuid().ToString(),
				Name = phone.Name,
				Description = phone.Description,
				Price = phone.Price,
				DateCreated = phone.DateCreated,
				Exires = phone.Exires
			};

			var response = await _context.Phones.AddAsync(phoneResponse);
			await _context.SaveChangesAsync();

			return response != null;
		}

		public async Task<PhoneResponse> GetPhoneInfoByIdAsync(string Id)
		{
			var response = await _context.Phones.FindAsync(Id);
			return response;
		}

		public async Task<IEnumerable<PhoneResponse>> SearchPhonesAsync(string search)
		{
			var response = await _context.Phones.FindAsync(search);
			return new List<PhoneResponse> { response };
		}
	}
}
