using System.Collections.Generic;
using Newtonsoft.Json;
using OrkJkh.Core.Api.Controllers;
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

		public Dictionary<string, BuildInfo> BuildData { get; set; }

		public List<uint> Meters { get; set; }

		public Dictionary<string, EventRecord> Feed { get; set; }

		public UserDataRS(AppUser user)
		{
			FullName = user.FullName;
			Appartament = user.Appartament;
			Phone = user.PhoneNumber;
			Email = user.Email;
			BuildingIds = user.BuildingIds;
			Inn = user.Inn;
			Meters = user.MeterIds;
			UserType = user.UserType == UserEnum.B2C ? "B2C" : "B2B";
		}
	}
}