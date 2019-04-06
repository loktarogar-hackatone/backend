using System;
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
using OrkJkh.Core.Api.Models.Api;
using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
		
		public AuthController(UserManager<AppUser> userManager, 
				SignInManager<AppUser> signInManager, 
				IConfiguration configuration)
		{
			_userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
		}

		[HttpPost("register/b2c")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterRQ request)
		{
			var user = new AppUser();
			user.Email = request.Email;
			user.FullName = request.FullName;
			user.UserName = request.Email;
			user.PhoneNumber = request.Phone;
			user.Appartament = request?.Appartament;
			user.BuildingId = request?.BuildingId;

			var result = await _userManager.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);
				var token = GenerateJSONWebToken(user);

				var rootData = new RegisterRS(token, user.Email);
				return Created("api/auth/register", rootData);
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
			
            var userData = new UserDataRS(user);

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
}
