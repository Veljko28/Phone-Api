using Phone_Api.Models;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IPurchaseRepository
	{
		Task<GenericResponse> AddPurchaseAsync(IEnumerable<PurchaseRequest> req);
		Task<IEnumerable<string>> GetPurchasedPhonesPageAsync(string userId, int page);
		Task<int> GetNumOfPagesAsync(string userId);
	}
}
