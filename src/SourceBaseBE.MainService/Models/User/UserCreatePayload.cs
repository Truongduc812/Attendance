using Microsoft.AspNetCore.Http;
using System;
using SourceBaseBE.Database.Entities;

namespace SourceBaseBE.MainService.Models.User
{
	public class UserCreatePayload
	{
		public long? Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Sex { get; set; }
		public string? BirthDay { get; set; }
		public string? Role { get; set; }
		public string? Station { get; set; }
		public string? Address { get; set; }
	}
}
