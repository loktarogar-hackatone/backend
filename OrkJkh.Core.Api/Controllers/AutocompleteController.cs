using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OrkJkh.Core.Api.Models;
using SharedModels;

namespace OrkJkh.Core.Api.Controllers
{
	[Route("api/autocomplete")]
	[ApiController]
	public class AutocompleteController : ControllerBase
	{
		private static readonly Regex _validSymbolsRegex = new Regex(@"[а-яё\s\d]*", RegexOptions.Compiled);


		private readonly IMongoCollection<HouseDto> _houseCollection;
		
		private  readonly IMongoCollection<HouseAutocompleteDto> _autocompleteCollection;

		
		public AutocompleteController(IConfiguration config)
		{
			var client = new MongoClient(config["MongoConnectionString"]);
			var database = client.GetDatabase("orkjkh");
			
			_houseCollection = database.GetCollection<HouseDto>("house_data");
			_autocompleteCollection = database.GetCollection<HouseAutocompleteDto>("house_autocomplete");
		}

		
		[HttpGet("house")]
		public async Task<IActionResult> GetHouse(string address)
		{
			if (address.Length < 3)
			{
				return Ok(new List<object>());
			}

			string normalizedAddress = NormalizeAddress(address)
				.Replace(" ", ".*");
			var regex = new BsonRegularExpression(normalizedAddress);
			FilterDefinition<HouseAutocompleteDto> filter = 
				Builders<HouseAutocompleteDto>.Filter.Regex("NormalizedAddress", regex);
			
			var rawData = await _autocompleteCollection
				.Find(filter)
				.Limit(10)
				.ToListAsync();

			var data = rawData.Select(d => new { Id = d.HouseId, Address = d.Address });
			
			return Ok(data);
		}

		[HttpGet("rebuild")]
		public async Task<IActionResult> Rebuild()
		{
			await _autocompleteCollection.DeleteManyAsync(_ => true);

			var houses = _houseCollection.AsQueryable();

			foreach (var house in houses)
			{
				var houseAutocompleteDto = new HouseAutocompleteDto();

				houseAutocompleteDto.HouseId = house.MongoId;
				houseAutocompleteDto.Address = house.address;
				houseAutocompleteDto.NormalizedAddress = NormalizeAddress(house.address);
				
				await _autocompleteCollection.InsertOneAsync(houseAutocompleteDto);
			}
			
			return Ok();
		}
		
		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			List<HouseAutocompleteDto> data = await _autocompleteCollection.Find(_ => true).ToListAsync();
			return Ok(data);
		}


		private static string NormalizeAddress(string rawAddress)
		{
			return string.Join("", _validSymbolsRegex.Matches(rawAddress.ToLower()));
		}
	}
}