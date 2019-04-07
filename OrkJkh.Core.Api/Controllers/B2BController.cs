using System.Collections.Generic;
using System.Net;
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
	[Route("api/b2b")]
	public class B2BController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

		public B2BController(UserManager<AppUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[Route("add")]
		public async Task<IActionResult> AddBuilding(string buildingId)
		{
			var user = await _userManager.GetUserAsync(User);

			if (!user.IsB2B()) 
				return Unauthorized();

			if (user.BuildingIds == null)
				user.BuildingIds = new List<string>();

			user.BuildingIds.Add(buildingId);

			await _userManager.UpdateAsync(user);

			return Ok();
		}
	}
}