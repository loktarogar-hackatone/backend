using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api.Models.Api
{
	public class UserDataRS
	{
		public string FullName { get; set; }

		public string BuildingId { get; set; }

		public string Appartament { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public UserDataRS(AppUser user)
		{
			FullName = user.FullName;
			Appartament = user.Appartament;
			Phone = user.PhoneNumber;
			Email = user.Email;
			BuildingId = user.BuildingId;
		}
	}
}