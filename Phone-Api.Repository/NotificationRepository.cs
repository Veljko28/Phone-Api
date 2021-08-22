using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly IConfiguration _configuration;

		public NotificationRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<GenericResponse> AddNotificationAsnyc(NotificationModelRequest model)
		{
			NotificationModel responseModel = new NotificationModel
			{
				Id = Guid.NewGuid().ToString(),
				Type = model.Type,
				Name = model.Name,
				UserId = model.UserId,
				Message = model.Message
			};

			string sql = "exec [_spAddNotifications] @Id, @Type, @Name, @UserId, @Message";

			return await DatabaseOperations.GenericExecute(sql, responseModel, _configuration, "Failed to add notification");
		}

		public async Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(string userId)
		{
			string sql = "exec [_spGetUserNotifications] @Id";

			return await DatabaseOperations.GenericQueryList<dynamic, NotificationModel>(sql, new { Id = userId }, _configuration);
		}

		public async Task<GenericResponse> RemoveNotificationAsnyc(NotificationModelRequest model)
		{

			string sql = "exec [_spRemoveNotifications] @Type, @Name, @UserId, @Message";

			return await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to remove notification");
		}
	}
}
