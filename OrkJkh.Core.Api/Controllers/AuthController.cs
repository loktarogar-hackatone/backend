﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using OrkJkh.Core.Api.Models.Api;
using OrkJkh.Core.Api.Models.Identity;
using SharedModels;

namespace OrkJkh.Core.Api.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
		private readonly IMongoCollection<HouseDto> _buildings;
		private readonly IMongoCollection<ManagementCompanyDto> _mc;
		private readonly IMongoCollection<EventRecord> _events;
		
		public AuthController(UserManager<AppUser> userManager, 
				SignInManager<AppUser> signInManager, 
				IConfiguration configuration)
		{
			_userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

			var client = new MongoClient(configuration["MongoConnectionString"]);
			var database = client.GetDatabase("orkjkh");

			_buildings = database.GetCollection<HouseDto>("house_data");
			_mc = database.GetCollection<ManagementCompanyDto>("managementcompany_data");
			_events = database.GetCollection<EventRecord>("events");
		}

		[HttpPost("register/b2c")]
		[AllowAnonymous]
		public async Task<IActionResult> RegisterB2C([FromBody] RegisterB2CRQ request)
		{
			var user = new AppUser();
			user.Email = request.Email;
			user.FullName = request.FullName;
			user.UserName = request.Email;
			user.PhoneNumber = request.Phone;
			user.Appartament = request?.Appartament;

			if (request.BuildingId != null)
			{
				user.BuildingIds = new List<string>();
				user.BuildingIds.Add(request.BuildingId);
			}

			var result = await _userManager.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);
				var token = GenerateJSONWebToken(user);

				var rootData = new RegisterB2CRS(token, user.Email);
				return Created("api/auth/register/b2c", rootData);
			}

			return BadRequest();
		}

		[HttpPost("register/b2b")]
		[AllowAnonymous]
		public async Task<IActionResult> RegisterB2B([FromBody] RegisterB2BRQ request)
		{
			var user = new AppUser();
			user.Email = request.Email;
			user.UserName = request.Email;
			user.Inn = request.Inn;
			user.UserType = UserEnum.B2B;
			user.PhoneNumber = request.Phone;
			user.FullName = request.FullName;

			var result = await _userManager.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);
				var token = GenerateJSONWebToken(user);

				var rootData = new { token, user.Inn, user.Email };
				return Created("api/auth/register/b2b", rootData);
			}

			return BadRequest();
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRQ request)
		{
			var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
			if (result.Succeeded)
			{
				var appUser = _userManager.Users.SingleOrDefault(r => r.Email == request.Email);
				var token = GenerateJSONWebToken(appUser);

				var rootData = new LoginRS(token, appUser.UserName, appUser.Email);
				return Ok(rootData);
			}

			return StatusCode((int)HttpStatusCode.Unauthorized, "Bad credentials");
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<ActionResult> UserData()
        {
            var user = await _userManager.GetUserAsync(User);
			
			var buildData = new Dictionary<string, BuildInfo>();
			if (user.BuildingIds != null && user.BuildingIds.Count > 0)
			{
				foreach (var buildId in user.BuildingIds)
				{
					var filter = Builders<HouseDto>.Filter.Eq("_id", new ObjectId(buildId));
					var buildRaw = (await _buildings.Find(filter).ToListAsync()).FirstOrDefault();
					if (buildRaw == null) continue;
					
					var mcRaw = (await _mc.Find(x => x.id == buildRaw.management_organization_id).ToListAsync()).FirstOrDefault();
					if (mcRaw == null) continue;

					buildData.Add(buildId, new BuildInfo { Address = buildRaw.address, ManagementCompany = mcRaw.name_short });
				}
			}

			var userData = new UserDataRS(user);
			userData.BuildData = buildData;

			if (user.BuildingIds != null && user.BuildingIds.Count > 0)
			{
				foreach (var buildId in user.BuildingIds)
				{
					var feedEvents = new List<EventRecord>();
					var filterBuilds = Builders<EventRecord>.Filter.Eq(x => x.BuildingId, buildId);
					var eventsRaw = await _events.Find(filterBuilds).ToListAsync();

					foreach (var eventRec in eventsRaw)
					{
						feedEvents.Add(new EventRecord 
						{
							BuildingId = eventRec.BuildingId,
							Owner = user.FullName,
							Text = eventRec.Text,
							CreateDate = eventRec.CreateDate,
						});
					}

					if (feedEvents.Count > 0)
					{
						if (userData.Feed == null) userData.Feed = new List<EventRecord>();

						foreach (var feedEnv in feedEvents)
						{
							userData.Feed.Add(feedEnv);
						}
					}
				}
			}

            return Ok(userData);
        }

		private string GenerateJSONWebToken(AppUser user)  
        {  
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        } 
	}

	public class BuildInfo
	{
		public string Address { get; set; }
		
		public string ManagementCompany { get; set; }
	}
}
