using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api.Models.Api
{
	public class UserDataRS
	{
		public string FirstName { get; set; }

		public string SecondName { get; set; }

		public string BuildingId { get; set; }

		public string Email { get; set; }

		public UserDataRS(AppUser user)
		{
			FirstName = user.FirstName;
			SecondName = user.SecondName;
			Email = user.Email;
			BuildingId = user.BuildingId;
		}
	}
}