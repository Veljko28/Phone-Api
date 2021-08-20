using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface INotificationRepository
	{
		Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(string userId);

		Task<GenericResponse> AddNotificationAsnyc(NotificationModelRequest model);
		Task<GenericResponse> RemoveNotificationAsnyc(string Id);

	}
}
