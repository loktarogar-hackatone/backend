using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrkJkh.Core.Api.Models
{
	public class DataDto
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("UniqueIdentifier")]
		public uint UniqueIdentifier { get; set; }
		
		[BsonElement("Value")]
		public uint Value { get; set; }
		
		[BsonElement("MeterType")]
		public MeterType MeterType { get; set; }
		
		[BsonElement("InsertionDateTime")]
		public DateTime InsertionDateTime { get; set; }
	}

	public enum MeterType : byte
	{
		Water = 0,
		Electric  = 1,
		Gas = 2
	}
}