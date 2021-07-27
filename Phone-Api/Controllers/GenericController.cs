﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Phone_Api.Models.Requests;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using Phone_Api.Repository.Helpers;
using Phone_Api.Models;

namespace Phone_Api.Controllers
{
	public class GenericController : Controller
	{
        private readonly IWebHostEnvironment environment;
		private readonly IConfiguration _configuration;

		public GenericController(IWebHostEnvironment _environment, IConfiguration configuration)
        {
            environment = _environment;
			_configuration = configuration;
		}

        public class FormUpload
        {
            [NotMapped]
            public IFormFile Files { get; set; }
        };

        public async Task<bool> ImageUploadFunc(string sql, IFormFile upload, UploadRequest req)
		{
            try
            {
                if (upload.Length > 0)
                {
                    if (!Directory.Exists(environment.WebRootPath + "\\Uploads\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\Uploads\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(environment.WebRootPath + "\\Uploads\\" + upload.FileName))
                    {
                        await upload.CopyToAsync(fileStream);

                        await fileStream.FlushAsync();

                        string imagePath = "http://localhost:10025" + "/Uploads/" + upload.FileName;

                        using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                        {
                            await db.OpenAsync();

                            int rowsModified = await db.ExecuteAsync(sql, new { req.Id, Image = imagePath });

                            if (rowsModified == 0) return false;


                            return true;
                        }

                    }
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }


        public async Task<bool> UserImageUploadFunc(string sql, FormUpload upload, string Id)
        {
            try
            {
                if (upload.Files.Length > 0)
                {
                    if (!Directory.Exists(environment.WebRootPath + "\\Uploads\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\Uploads\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(environment.WebRootPath + "\\Uploads\\" + upload.Files.FileName))
                    {
                        await upload.Files.CopyToAsync(fileStream);

                        await fileStream.FlushAsync();

                        string imagePath = "http://localhost:10025" + "/Uploads/" + upload.Files.FileName;

                        using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                        {
                            await db.OpenAsync();

                            int rowsModified = await db.ExecuteAsync(sql, new { Id, Image = imagePath });

                            if (rowsModified == 0) return false;


                            return true;
                        }

                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }



        [HttpPost(ApiRoutes.GenericRoutes.UserImageUpload)]
        public async Task<IActionResult> UserImageUpload([FromForm] FormUpload upload, [FromForm] string Id)
        {
            string sql = "exec [_spUserImageUpload] @Id, @Image";

            var succeded = await UserImageUploadFunc(sql, upload, Id);

            if (succeded)
			{
                return Ok("Successfully added your image");
			}

            return BadRequest("Failed to add your image");
        }


        [HttpPost(ApiRoutes.GenericRoutes.PhoneBidImageUpload)]
        public async Task<IActionResult> PhoneImageUpload([FromForm] IList<IFormFile> Files, [FromForm] UploadRequest req)
		{
            string sql;
            if (req.Type == uploadTypes.Phone)
			{
                sql = "exec [_spPhoneImageUpload] @Id, @Image";
            }
            else sql = "exec [_spBidImageUpload] @Id, @Image";

			foreach (var image in Files)
			{
				var succeded = await ImageUploadFunc(sql, image, req);

                if (!succeded)
				{
                    return BadRequest("Failed to add all images");
				}
			}

			return Ok("Successfully added all images");
        }

        [HttpPost(ApiRoutes.GenericRoutes.Contact)]
        public async Task<IActionResult> Contact([FromBody] ContactRequest req)
		{
            string sql = "exec [_spContactSupport] @Id, @Name, @Email, @Subject, @Message";

            ContactModel model = new ContactModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = req.Name,
                Email = req.Email,
                Subject = req.Subject,
                Message = req.Message
            };

            var result = await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to send the support message");

            if (!result.Success)
			{
                return BadRequest(result.ErrorMessage);
			}

            return Ok("Your message has been sent !");
		}

        public class EmailModel
		{
			public string Email { get; set; }
		}


        [HttpPost(ApiRoutes.GenericRoutes.Subscribe)]
        public async Task<IActionResult> Subscribe([FromBody] EmailModel email)
		{
            SubscribeModel model = new SubscribeModel
            {
                Id = Guid.NewGuid().ToString(),
                Email = email.Email
            };

            string sql = "exec [_spAddSubscribeEmail] @Id, @Email";

            var result = await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to add email to database");

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Your email has been sent !");
        }

        [HttpPost(ApiRoutes.GenericRoutes.Search)]
        public async Task<IActionResult> Search([FromRoute] string term)
		{
            term = InjestionProtect.StringSqlRemove(term);

            const string phoneSql = "exec [_spSearchPhones] @Term";
            const string bidSql = "exec [_spSearchBids] @Term";

            var phones = await DatabaseOperations.GenericQueryList<dynamic, PhoneModel>(phoneSql, new { Term = '%' + term + '%'}, _configuration);
            var bids = await DatabaseOperations.GenericQueryList<dynamic, BidModel>(bidSql, new { Term = '%' + term + '%' }, _configuration);

            SearchModel model = new SearchModel
            {
                Phones = phones,
                Bids = bids
            };

            return Ok(model);
		}
    }
}
