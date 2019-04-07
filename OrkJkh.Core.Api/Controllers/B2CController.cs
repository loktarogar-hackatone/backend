using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrkJkh.Core.Api.Models;
using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api.Controllers
{
	[ApiController]
	[Route("api/b2c")]
	public class B2CController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		
		private readonly IMongoCollection<DataDto> _collection;

		public B2CController(UserManager<AppUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			
			var client = new MongoClient(configuration["MongoConnectionString"]);
			var database = client.GetDatabase("orkjkh");
			
			_collection = database.GetCollection<DataDto>("data");
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("addmeter")]
		public async Task<IActionResult> AddMeter(uint meterId)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user.IsB2B()) 
				return Unauthorized();

			if (user.MeterIds == null)
				user.MeterIds = new List<uint>();

			user.MeterIds.Add(meterId);

			await _userManager.UpdateAsync(user);

			return Ok();
		}
		
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("meters")]
		public async Task<IActionResult> GetMeters()
		{
			var user = await _userManager.GetUserAsync(User);
			var meterIds = new List<uint>();

			if (user.IsB2B()) 
				return Unauthorized();

			if (user.MeterIds == null)
			{
				return Ok();
			}
			else
			{
				meterIds = user.MeterIds;
			}

			FilterDefinition<DataDto> filter =
				Builders<DataDto>.Filter.Where(d => meterIds.Contains(d.UniqueIdentifier));
			
			List<DataDto> meters = await _collection
				.Find(filter)
				.ToListAsync();

			var result = meterIds.Select(mi => new
			{
				UniqueIdentifier = mi, 
				MeasurementType = meters.First(m => m.UniqueIdentifier == mi).MeasurementType
			});

			return Ok(result);
		}
	}
}