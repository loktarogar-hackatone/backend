using System.Collections.Generic;
using Newtonsoft.Json;
using OrkJkh.Core.Api.Models.Identity;

namespace OrkJkh.Core.Api.Models.Api
{
	public class UserDataRS
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string FullName { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<string> BuildingIds { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Appartament { get; set; }

		public string Email { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Phone { get; set; }

		public string UserType { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Inn { get; set; }

		public UserDataRS(AppUser user)
		{
			FullName = user.UserName;
			Appartament = user.Appartament;
			Phone = user.PhoneNumber;
			Email = user.Email;
			BuildingIds = user.BuildingIds;
			Inn = user.Inn;
			UserType = user.UserType == UserEnum.B2C ? "B2C" : "B2B";
		}
	}
}