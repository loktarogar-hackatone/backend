using System.ComponentModel.DataAnnotations;

namespace OrkJkh.Core.Api.Models.Api
{
	public class RegisterB2CRQ
	{
		[Required]
		public string FullName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string Phone { get; set; }

		public string Appartament { get; set; }

		public string BuildingId { get; set; }
	}
}