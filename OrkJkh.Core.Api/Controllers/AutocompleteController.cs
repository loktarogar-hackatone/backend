using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SharedModels;

namespace OrkJkh.Core.Api.Controllers
{
	[Route("api/autocomplete")]
	[ApiController]
	public class AutocompleteController : ControllerBase
	{
		private readonly IMongoCollection<HouseDto> _collection;

		
		public AutocompleteController(IConfiguration config)
		{
			var client = new MongoClient(config["MongoConnectionString"]);
			var database = client.GetDatabase("orkjkh");
			_collection = database
				.GetCollection<HouseDto>("house_data");
		}

		
		[HttpGet("house")]
		public async Task<IActionResult> GetHouse(string address)
		{
			var regex = new BsonRegularExpression(address, "i");
			FilterDefinition<HouseDto> filter = Builders<HouseDto>.Filter.Regex("address", regex);
			
			var rawData = await _collection
				.Find(filter)
				.Limit(10)
				.ToListAsync();

			var data = rawData.Select(d => new { Id = d.MongoId, Address = d.address });
			
			return Ok(data);
		}
	}
}