namespace ActiveClub.Firma.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int? UserId { get; private set; }
        public string? UserName { get; private set; }
        public string? FullName { get; private set; }
        public string? Stanowisko { get; private set; }
        public string? Rola { get; private set; }

        public bool IsLoggedIn => UserId.HasValue;

        public bool IsAdmin => string.Equals(Rola, "Admin", StringComparison.OrdinalIgnoreCase);
        public bool IsPracownik => string.Equals(Rola, "Pracownik", StringComparison.OrdinalIgnoreCase);
        public bool IsTrener => string.Equals(Rola, "Trener", StringComparison.OrdinalIgnoreCase);
        public bool IsKlient => string.Equals(Rola, "Klient", StringComparison.OrdinalIgnoreCase);

        public void SetCurrentUser(
            int userId,
            string userName,
            string fullName,
            string? stanowisko,
            string? rola)
        {
            UserId = userId;
            UserName = userName;
            FullName = fullName;
            Stanowisko = stanowisko;
            Rola = rola;
        }

        public void Clear()
        {
            UserId = null;
            UserName = null;
            FullName = null;
            Stanowisko = null;
            Rola = null;

        }
        //do usuniecia

        public void SetRole(string role)
        {
            Rola = role;
        }
    }
}