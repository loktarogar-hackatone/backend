namespace OrkJkh.Core.Api.Models.Api
{
	public class RegisterB2CRS
	{
		public string Token { get; set; }
		
		public string Email { get; set; }

		public RegisterB2CRS(string token, string email)
		{
			Token = token;
			Email = email;
		}
	}
}