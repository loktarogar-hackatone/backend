using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrkJkh.Core.Api.Models
{
	public class HouseAutocompleteDto
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		
		[BsonElement]
		public string HouseId { get; set; }
		
		[BsonElement]
		public string Address { get; set; }
		
		[BsonElement]
		public string NormalizedAddress { get; set; }
	}
}