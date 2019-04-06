using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrkJkh.Core.Api.Controllers
{
	[Route("api/values")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		// GET api/values
		[AllowAnonymous]
		[HttpGet("getall")]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { "value1", "lol" };
		}

		// GET api/values/5
		[AllowAnonymous]
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id) => "value";

		// POST api/values
		[AllowAnonymous]
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[AllowAnonymous]
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[AllowAnonymous]
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
