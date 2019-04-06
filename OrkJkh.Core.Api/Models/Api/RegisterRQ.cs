using System.ComponentModel.DataAnnotations;

namespace OrkJkh.Core.Api.Models.Api
{
	public class RegisterRQ
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string SecondName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public string BuildingId { get; set; }


	}
}