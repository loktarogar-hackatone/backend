using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api.Controllers
{
	[ApiController]
	[Route("api/b2c")]
	public class B2CController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

		public B2CController(UserManager<AppUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
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
			var result = new List<uint>();

			if (user.IsB2B()) 
				return Unauthorized();

			if (user.MeterIds != null)
				result = user.MeterIds;

			return Ok(result);
		}
	}
}