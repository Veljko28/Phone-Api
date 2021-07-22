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
		Task<string> AddPurchaseAsync(PurchaseRequest req);
		Task<IEnumerable<string>> GetPurchasePhonesAsync(string purchaseId);
		Task<GenericResponse> AddPhoneToPurchaseAsync(string purchaseId, string phoneId);


	}
}
