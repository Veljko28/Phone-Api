using Microsoft.AspNetCore.Hosting;
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

        [HttpPost(ApiRoutes.GenericRoutes.ImageUpload)]
        public async Task<IActionResult> ImageUpload([FromForm] FormUpload upload, [FromForm] UploadRequest req)
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

                        string imagePath = "https://localhost:44396" + "/Uploads/" + upload.Files.FileName;

                        using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
						{
                            await db.OpenAsync();

                            if (req.Type == uploadTypes.User)
							{
                               int rowsModified = await db.ExecuteAsync("exec [_spUserImageUpload] @Id, @Image", new { req.Id, Image = imagePath });

                               if (rowsModified == 0) return BadRequest("Unable to upload this image");
							}
                            else
							{
                                int rowsModified = await db.ExecuteAsync("exec [_spPhoneImageUpload] @Id, @Image", new { req.Id, Image = imagePath });

                                if (rowsModified == 0) return BadRequest("Unable to upload this image");
                            }

                            return Ok("Successfully Uploaded your image");
						}

                    }
                }
                else
                {
                    return BadRequest("Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
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
