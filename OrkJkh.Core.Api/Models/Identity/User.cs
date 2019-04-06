using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;

namespace OrkJkh.Core.Api.Models.Identity
{
	public class Role : MongoRole
	{
	}

	public class AppUser : MongoUser
	{
		public string FirstName { get; set; }

		public string SecondName { get; set; }

		public string BuildingId { get; set; }
	}
}