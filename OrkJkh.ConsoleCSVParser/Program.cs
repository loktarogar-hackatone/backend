using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using SharedModels;

namespace OrkJkh.ConsoleCSVParser
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				throw new ArgumentException("Specify both paths to houses & management companies csvs");
			}
			
			Console.WriteLine("Hello to csv parser!");

			string houseCsvPath = args[0];
			string managementCompanyCsvPath = args[1];

			MongoClient client = new MongoClient("mongodb://35.228.126.23:27017");
			IMongoDatabase database = client.GetDatabase("orkjkh");

			if (!string.IsNullOrEmpty(houseCsvPath))
			{
				UploadHouseData(houseCsvPath, database);
			}

			if (!string.IsNullOrEmpty(managementCompanyCsvPath))
			{
				UploadManagementCompanyData(managementCompanyCsvPath, database);
			}
		}


		private static void UploadHouseData(string csvPath,IMongoDatabase database)
		{
			IMongoCollection<HouseDto> collection = database.GetCollection<HouseDto>("house_data");
			
			using (var reader = new StreamReader(csvPath, Encoding.UTF8))
			{
				using (var csv = new CsvReader(reader))
				{
					// To ignore bad data
					csv.Configuration.BadDataFound = null;
					csv.Configuration.MissingFieldFound = null;
					
					csv.Configuration.RegisterClassMap<HouseDtoMap>();
					IEnumerable<HouseDto> records = csv.GetRecords<HouseDto>();
					
					foreach (var record in records)
					{
						collection.InsertOne(record);
					}
				}
			}
			
			Console.WriteLine("House data upload finished!");
		}
		
		private static void UploadManagementCompanyData(string csvPath,IMongoDatabase database)
		{
			IMongoCollection<ManagementCompanyDto> collection = 
				database.GetCollection<ManagementCompanyDto>("managementcompany_data");
			
			using (var reader = new StreamReader(csvPath, Encoding.UTF8))
			{
				using (var csv = new CsvReader(reader))
				{
					// To ignore bad data
					csv.Configuration.BadDataFound = null;
					csv.Configuration.MissingFieldFound = null;
					
					csv.Configuration.RegisterClassMap<ManagementCompanyDtoMap>();
					IEnumerable<ManagementCompanyDto> records = csv.GetRecords<ManagementCompanyDto>();
					
					foreach (var record in records)
					{
						collection.InsertOne(record);
					}
				}
			}
			
			Console.WriteLine("Management Company data upload finished!");
		}
	}

	
	public sealed class HouseDtoMap : ClassMap<HouseDto>
	{
		public HouseDtoMap()
		{
			AutoMap();
			Map(m => m.MongoId).Ignore();
		}
	}
	
	public sealed class ManagementCompanyDtoMap : ClassMap<ManagementCompanyDto>
	{
		public ManagementCompanyDtoMap()
		{
			AutoMap();
			Map(m => m.MongoId).Ignore();
		}
	}
}