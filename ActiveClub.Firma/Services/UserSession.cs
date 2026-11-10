namespace ActiveClub.Firma.Services
{
    public class UserSession
    {
        public string? Token { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

        public void SetToken(string token)
        {
            Token = token;
        }

        public void Clear()
        {
            Token = null;
        }
    }
}