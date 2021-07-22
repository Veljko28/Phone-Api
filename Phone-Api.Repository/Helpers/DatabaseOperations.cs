using Dapper;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Helpers
{
	public static class DatabaseOperations
	{

		public static async Task<GenericResponse> GenericExecute<T>(string sql, T payload, IConfiguration _configuration, string ErrorMessage)
		{
			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql, payload);

				if (rowsModified != 0) return new GenericResponse { Success = true };

				return new GenericResponse { Success = false, ErrorMessage = ErrorMessage };
			}
		}

		public static async Task<IEnumerable<N>> GenericQueryList<T,N>(string sql, T payload, IConfiguration _configuration)
		{
			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				IEnumerable<N> list = await db.QueryAsync<N>(sql, payload);

				return list;
			}
		}


		public static async Task<N> GenericQuerySingle<T,N>(string sql, T payload, IConfiguration _configuration)
		{
			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				N list = (await db.QueryAsync<N>(sql, payload)).FirstOrDefault();

				return list;
			}
		}



	}
}
