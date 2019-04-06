using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentityMongoDbProvider<AppUser, Role>(
			conf => {
				conf.Password.RequireDigit = false;
				conf.Password.RequireLowercase = false;
				conf.Password.RequireNonAlphanumeric = false;
				conf.Password.RequireUppercase = false;
				conf.Password.RequiredLength = 4;
				conf.Password.RequiredUniqueChars = 0;
				conf.ClaimsIdentity.UserNameClaimType = ClaimTypes.Email;
			},
			opt =>
			{
				opt.ConnectionString = Configuration["MongoConnectionString"];
			});

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

			services.AddAuthentication(options =>
            {
                //Set default Authentication Schema as Bearer
                options.DefaultAuthenticateScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters =
						new TokenValidationParameters
						{
							ValidIssuer = Configuration["JwtIssuer"],
							ValidAudience = Configuration["JwtIssuer"],
							IssuerSigningKey =
								new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
							ClockSkew = TimeSpan.Zero
						};
            });

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseAuthentication();
			//app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
