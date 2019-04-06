namespace OrkJkh.Core.Api.Models.Api
{
	public class RegisterB2CRS
	{
		public string Token { get; set; }
		
		public string Name { get; set; }

		public RegisterB2CRS(string token, string name)
		{
			Token = token;
			Name = name;
		}
	}
}