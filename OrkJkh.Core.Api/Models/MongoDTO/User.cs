using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrkJkh.Core.Api.Models.MongoDTO
{
	public class UserDto
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string FirstName { get; set; }

		public string SecondName { get; set; }

		public string Email { get; set; }
	}
}