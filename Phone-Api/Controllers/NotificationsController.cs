using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class NotificationsController : Controller
	{
		private readonly INotificationRepository _notifications;
		public NotificationsController(INotificationRepository notifications)
		{
			_notifications = notifications;
		}


		[HttpGet(ApiRoutes.NotificationsRoutes.GetUserNotifications)]
		public async Task<IActionResult> GetUserNotifications([FromRoute] string userId)
		{
			IEnumerable<NotificationModel> models = await _notifications.GetUserNotificationsAsync(userId);

			if (models == null)
			{
				return BadRequest("Failed to get any notifications");
			}

			return Ok(models);
		}

		[HttpPost(ApiRoutes.NotificationsRoutes.AddNotification)]
		public async Task<IActionResult> AddNotification([FromBody] NotificationModelRequest model)
		{
			GenericResponse response = await _notifications.AddNotificationAsnyc(model);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}


		[HttpPost(ApiRoutes.NotificationsRoutes.RemoveNotification)]
		public async Task<IActionResult> RemoveNotification([FromRoute] string notificationId)
		{
			GenericResponse response = await _notifications.RemoveNotificationAsnyc(notificationId);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}



	}
}
