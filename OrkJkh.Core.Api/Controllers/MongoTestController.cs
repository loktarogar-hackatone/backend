using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace OrkJkh.Core.Api.Controllers
{
	[Route("api/mongotest")]
	[ApiController]
	public class MongoTestController : ControllerBase
	{
		private readonly IMongoCollection<MongoTestDto> _collection;

		public MongoTestController(IConfiguration config)
		{
			var client = new MongoClient("mongodb://35.228.126.23:27017");
			var database = client.GetDatabase("orkjkh");
			_collection = database.GetCollection<MongoTestDto>("items");
		}

		[HttpGet("insert")]
		public async Task<IActionResult> Insert(string item)
		{
			await _collection.InsertOneAsync(new MongoTestDto { Item = item });
			return Ok();
		}

		[HttpGet("getall")]
		public async Task<IActionResult> GetThemAll()
		{
			var data = await _collection.Find(x => true).ToListAsync();
			return Ok(data);
		}
	}

	public class MongoTestDto
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("Item")]
		public string Item { get; set; }
	}
}
