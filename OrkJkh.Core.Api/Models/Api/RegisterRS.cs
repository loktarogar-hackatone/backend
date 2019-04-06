namespace OrkJkh.Core.Api.Models.Api
{
	public class RegisterRS
	{
		public string Token { get; set; }
		
		public string Name { get; set; }

		public RegisterRS(string token, string name)
		{
			Token = token;
			Name = name;
		}
	}
}