using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using OrkJkh.Core.Api.Models;
using OrkJkh.Core.Api.Models.Identity;
using Remotion.Linq.Clauses;

namespace OrkJkh.Core.Api.Controllers
{
	[Route("api/data")]
	[ApiController]
	public class DataReceiverController : ControllerBase
	{
		private const string DATE_TIME_FORMAT = "dd.MM.yyyyTHH:mm:ss";
		
		private const string DATE_FORMAT = "dd.MM.yyyy";

		
		private readonly IMongoCollection<DataDto> _collection;

		private readonly UserManager<AppUser> _userManager;

		
		public DataReceiverController(UserManager<AppUser> userManager, IConfiguration config)
		{
			var client = new MongoClient(config["MongoConnectionString"]);
			var database = client.GetDatabase("orkjkh");
			
			_collection = database.GetCollection<DataDto>("data");
			_userManager = userManager;
		}

		
		[HttpGet("insert")]
		public async Task<IActionResult> Insert(
			uint uniqueIdentifier, 
			uint value, 
			MeasurementType measurementType, 
			// For demonstration only
			string dateTime)
		{
			var dt = ParseDateTimeAsUtc(dateTime, DATE_TIME_FORMAT);
			
			await _collection.InsertOneAsync(new DataDto
			{
				UniqueIdentifier = uniqueIdentifier,
				Value = value,
				MeasurementType = measurementType,
				InsertionDate = dt.Date,
				InsertionDateTime = dt
			});
			
			return Ok();
		}
		
		[HttpGet("meter")]
		public async Task<IActionResult> GetMeter(uint uniqueIdentifier, string startDate, string endDate)
		{
			var startDt = ParseDateTimeAsUtc(startDate, DATE_FORMAT).Date;
			var endDt = ParseDateTimeAsUtc(endDate, DATE_FORMAT).Date;
			
			FilterDefinition<DataDto> filter = Builders<DataDto>.Filter.Where(d => 
				d.UniqueIdentifier == uniqueIdentifier &&
				d.InsertionDate >= startDt &&
				d.InsertionDate <= endDt);
			
			List<DataDto> rawData = await _collection
				.Find(filter)
				.ToListAsync();

			var data = rawData
				.GroupBy(d => d.InsertionDate)
				.Select(g => new
				{
					InsertionDateTime = g.Key.ToString(DATE_FORMAT, CultureInfo.InvariantCulture),
					Value = g.Sum(e => e.Value),
					MeasurementType = g.First().MeasurementType
				});
			
			return Ok(data);
		}
		
		[HttpGet("house")]
		public async Task<IActionResult> GetByHouse(
			string buildingId, 
			string startDate, 
			string endDate, 
			MeasurementType measurementType)
		{
			var meterIds = _userManager
				.Users
				.Where(u => u.BuildingIds != null && u.BuildingIds.Contains(buildingId))
				.Where(u => u.MeterIds != null)
				.SelectMany(u => u.MeterIds)
				.ToList();
			
			var startDt = ParseDateTimeAsUtc(startDate, DATE_FORMAT).Date;
			var endDt = ParseDateTimeAsUtc(endDate, DATE_FORMAT).Date;

			FilterDefinition<DataDto> filter = Builders<DataDto>.Filter.Where(d =>
				d.MeasurementType == measurementType &&
				meterIds.Contains(d.UniqueIdentifier) &&
				d.InsertionDate >= startDt &&
				d.InsertionDate <= endDt);
			
			List<DataDto> rawData = await _collection
				.Find(filter)
				.ToListAsync();

			var data = rawData
				.GroupBy(d => d.InsertionDate)
				.Select(g => new
				{
					InsertionDateTime = g.Key.ToString(DATE_FORMAT, CultureInfo.InvariantCulture),
					Value = g.Sum(e => e.Value)
				});
			
			
			return Ok(data);
		}

		[HttpGet("drop")]
		public async Task<IActionResult> Drop()
		{
			await _collection.DeleteManyAsync(_ => true);
			return Ok();
		}


		private DateTime ParseDateTimeAsUtc(string dateTime, string format)
		{
			return DateTime.SpecifyKind(
				DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture),
				DateTimeKind.Utc);
		}
	}
}