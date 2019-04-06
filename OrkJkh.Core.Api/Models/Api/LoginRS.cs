namespace OrkJkh.Core.Api.Models.Api
{
	public class LoginRS
	{
		public LoginRS(string token, string userName, string email)
        {
            Token = token;
            UserName = userName;
            Email = email;
        }

        public string Token { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }
	}
}