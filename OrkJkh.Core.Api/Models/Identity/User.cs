using System.Collections.Generic;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;

namespace OrkJkh.Core.Api.Models.Identity
{
	public class Role : MongoRole
	{
	}

	public class AppUser : MongoUser
	{
		public string FullName { get; set; }

		public string Appartament { get; set; }

		public List<string> BuildingIds { get; set; }

		public string Inn { get; set; }

		public UserEnum UserType { get; set; } = UserEnum.B2C;
	}

	public enum UserEnum
	{
		B2C,
		B2B
	}
}