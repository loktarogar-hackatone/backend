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

		public string BuildingId { get; set; }

		public UserType UserType { get; set; } = UserType.B2C;
	}

	public enum UserType
	{
		B2C,
		B2B
	}
}