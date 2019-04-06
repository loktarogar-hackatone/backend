using System;
using System.IO;
using CsvHelper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace OrkJkh.ConsoleCSVParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello to csv parser!");

			var client = new MongoClient("mongodb://35.228.126.23:27017");
			var database = client.GetDatabase("orkjkh");
			var collection = database.GetCollection<HouseDTO>("house_autocomplete");

			using (var reader = new StreamReader("/Users/fimcenkokirill/Downloads/export-reestrmkd-64-20190301 - export-reestrmkd-64-20190301.csv"))
			using (var csv = new CsvReader(reader))
			{
				var records = csv.GetRecords<dynamic>();
				foreach (var row in records)
				{
					//var id = 
				}
			}
        }
    }

	public class HouseDTO
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Region { get; set; }

		public string City { get; set; }

		public string Street { get; set; }

		public string Building { get; set; }
	}
}
