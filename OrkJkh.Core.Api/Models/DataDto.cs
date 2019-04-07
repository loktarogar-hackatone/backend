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
		public MeasurementType MeasurementType { get; set; }
		
		[BsonElement("InsertionDateTime")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime InsertionDateTime { get; set; }
		
		[BsonElement("InsertionDate")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime InsertionDate { get; set; }
	}

	public enum MeasurementType : byte
	{
		HotWater = 0,
		ColdWater = 1,
		Electric  = 2,
		Gas = 3
	}
}