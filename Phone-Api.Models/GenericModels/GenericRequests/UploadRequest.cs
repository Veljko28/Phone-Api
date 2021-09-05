using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{

	public enum uploadTypes
	{
		Phone,
		Bid
	}

	public class UploadRequest
	{
		public uploadTypes Type { get; set; }
		public string Id { get; set; }
	}
}
