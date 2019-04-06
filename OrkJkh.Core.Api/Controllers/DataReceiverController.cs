using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrkJkh.Core.Api.Models;

namespace OrkJkh.Core.Api.Controllers
{
	[Route("api/datareceiver")]
	[ApiController]
	public class DataReceiverController : ControllerBase
	{
		private readonly IMongoCollection<DataDto> _collection;

		
		public DataReceiverController(IConfiguration config)
		{
			var client = new MongoClient(config["MongoConnectionString"]);
			var database = client.GetDatabase("orkjkh");
			_collection = database.GetCollection<DataDto>("data");
		}

		[HttpGet("insert")]
		public async Task<IActionResult> Insert(uint uniqueIdentifier, uint value, MeterType meterType)
		{
			await _collection.InsertOneAsync(new DataDto
			{
				UniqueIdentifier = uniqueIdentifier,
				Value = value,
				MeterType = meterType,
				InsertionDateTime = DateTime.Now
			});
			
			return Ok();
		}
		
		[HttpGet("getall")]
		public async Task<IActionResult> GetThemAll()
		{
			var data = await _collection.Find(x => true).ToListAsync();
			return Ok(data);
		}
	}
}