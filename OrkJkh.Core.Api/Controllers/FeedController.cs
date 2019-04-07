using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api.Controllers
{
	[Route("api/feed")]
	[ApiController]
	public class FeedController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly IMongoCollection<EventRecord> _collection;

		public FeedController(UserManager<AppUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;

			var client = new MongoClient(configuration["MongoConnectionString"]);
			var database = client.GetDatabase("orkjkh");
			_collection = database.GetCollection<EventRecord>("items");
		}

		[HttpPost("new")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> AddEvent([FromBody] AddEventRQ request)
		{
			var user = await _userManager.GetUserAsync(User);
			
			var eventRec = new EventRecord { 
				Owner = user.Email,
				Text = request.Text,
				CreateDate = DateTime.Now,
			};

			await _collection.InsertOneAsync(eventRec);

			return Ok();
		}
	
		[HttpGet("list")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> List(string buildingId)
		{
			var user = await _userManager.GetUserAsync(User);

			var rawData = await _collection.Find(x => x.Owner == user.Email).ToListAsync();

			return Ok(rawData);
		}
	}

	public class EventRecord
	{
		public string Owner { get; set; }

		public string Text { get; set; }

		public DateTime CreateDate { get; set; }
	}

	public class AddEventRQ
	{
		// [Required]
		// public string Owner { get; set; }

		[Required]
		public string Text { get; set; }

		// [Required]
		// public DateTime CreateDate { get; set; }
	}
}