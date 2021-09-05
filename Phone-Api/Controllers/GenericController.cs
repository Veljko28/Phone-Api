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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;

namespace Phone_Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenericController : Controller
	{
        private readonly IWebHostEnvironment environment;
		private readonly IConfiguration _configuration;
        private readonly IUserRepository _users;
        private readonly IPhoneRepository _phones;

		public GenericController(IWebHostEnvironment _environment, IConfiguration configuration, IUserRepository users, IPhoneRepository phones)
		{
			environment = _environment;
			_configuration = configuration;
			_users = users;
			_phones = phones;
		}

		public class FormUpload
        {
            [NotMapped]
            public IFormFile Files { get; set; }
        };
        
        [HttpPost(ApiRoutes.GenericRoutes.PhoneDisplay)]
        public async Task<IActionResult> PhoneDisplay(FormUpload upload)
        {
            string filePath = Guid.NewGuid().ToString();
            string extention = upload.Files.FileName.Split('.')[1];
            string guidedFile = filePath + "." + extention;

            try
            {
                if (upload.Files.Length > 0)
                {
                    if (!Directory.Exists(environment.WebRootPath + "\\Uploads\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\Uploads\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(environment.WebRootPath + "\\Uploads\\" + guidedFile))
                    {
                        await upload.Files.CopyToAsync(fileStream);

                        await fileStream.FlushAsync();

                        string imagePath = "http://localhost:10025" + "/Uploads/" + guidedFile;

                        return Ok(imagePath);
                    }
                }
                else
                {
                    return BadRequest("Failed To Upload The Image");
                }
            }
            catch
            {
                return BadRequest("Failed To Upload The Image");
            }
        }

        public async Task<bool> ImageUploadFunc(string sql, IFormFile upload, UploadRequest req)
		{
            string filePath = Guid.NewGuid().ToString();
            string extention = upload.FileName.Split('.')[1];
            string guidedFile = filePath + "." + extention;

            try
            {
                if (upload.Length > 0)
                {
                    if (!Directory.Exists(environment.WebRootPath + "\\Uploads\\"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\Uploads\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(environment.WebRootPath + "\\Uploads\\" + guidedFile))
                    {
                        await upload.CopyToAsync(fileStream);

                        await fileStream.FlushAsync();

                        string imagePath = "http://localhost:10025" + "/Uploads/" + guidedFile;

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

            string DisplayImage = (await _phones.GetPhoneImagesAsync(req.Id)).FirstOrDefault();

            if (DisplayImage != null)
            {
                string updateSql = "exec [_spUpdateDisplayImage] @ImagePath, @PhoneId";

                await DatabaseOperations.GenericExecute(updateSql, new { ImagePath = DisplayImage, PhoneId = req.Id}, _configuration, "Failed to change display image");
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


        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet(ApiRoutes.GenericRoutes.Search)]
        public async Task<IActionResult> Search([FromRoute] string term)
		{
            term = InjestionProtect.StringSqlRemove(term);

            const string phoneSql = "exec [_spSearchPhones] @Term";
            const string bidSql = "exec [_spSearchBids] @Term";
            const string userSql = "exec [_spSearchUsers] @Term";

            var phones = await DatabaseOperations.GenericQueryList<dynamic, PhoneModel>(phoneSql, new { Term = '%' + term + '%'}, _configuration);
            var bids = await DatabaseOperations.GenericQueryList<dynamic, BidModel>(bidSql, new { Term = '%' + term + '%' }, _configuration);
            var users = await DatabaseOperations.GenericQueryList<dynamic, UserModel>(userSql, new { Term = '%' + term + '%' }, _configuration);

            SearchModel model = new SearchModel
            {
                Phones = phones,
                Bids = bids,
                Users = users
            };

            return Ok(model);
		}


        [HttpGet(ApiRoutes.GenericRoutes.ConfirmEmail)]
        public async Task<IActionResult> ConfirmUserEmail([FromRoute] string userId)
		{
            string sql = "exec [_spConfirmUserEmail] @Id";

            var user = await _users.GetUserByIdAsync(userId);

            if (user.EmailConfirmed)
			{
                return BadRequest("The email has already been confirmed");
			}

            GenericResponse response = await DatabaseOperations.GenericExecute(sql, new { Id = userId }, _configuration, "Failed to confirm email");

            if (!response.Success)
			{
                return BadRequest(response.ErrorMessage);
			}

            return Ok();
		}

        [HttpPost(ApiRoutes.GenericRoutes.CheckCoupon)]
        public async Task<IActionResult> CheckCoupon([FromBody] CouponRequest request)
		{
            string sql = "exec [_spFindUserCoupon] @Coupon";

            CouponModel model = await DatabaseOperations.GenericQuerySingle<dynamic, CouponModel>(sql, new { request.Coupon }, _configuration);

            if (model == null)
			{
                return NotFound("Failed to find the coupon");
			}

            if (model.UserId != request.UserId)
			{
                return BadRequest("This is not your coupon!");
			}

            if (!model.Valid)
			{
                return BadRequest("This coupon has already been used");
			}

            sql = "exec [_spInvalidateCoupon] @Id";

            GenericResponse response = await DatabaseOperations.GenericExecute(sql, new { model.Id }, _configuration, "Failed to invalidate coupon");

            if (!response.Success)
			{
                return BadRequest(response.ErrorMessage);
			}

            return Ok(model);
        }
    }
}
