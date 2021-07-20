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

                        string imagePath = "http://localhost:44396" + "/Uploads/" + upload.Files.FileName;

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
    }
}
