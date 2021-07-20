﻿using Phone_Api.Models;
using Phone_Api.Models.Requests;
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
		Task<IEnumerable<PhoneModel>> GetSellerPhonesByIdAsync(string sellerId);
	}
}