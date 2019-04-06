using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedModels
{
	public class ManagementCompanyDto
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string MongoId { get; set; }

		public string id { get; set; }
		public string subject_rf { get; set; }
		public string name_full { get; set; }
		public string name_short { get; set; }
		public string name_employee { get; set; }
		public string inn { get; set; }
		public string orn { get; set; }
		public string legal_address { get; set; }
		public string actual_address { get; set; }
		public string phone { get; set; }
		public string email { get; set; }
		public string site { get; set; }
		public string count_mkd { get; set; }
		public string area_total { get; set; }
		public string w_summ { get; set; }
	}
}