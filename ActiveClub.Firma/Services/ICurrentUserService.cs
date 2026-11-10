namespace ActiveClub.Firma.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? UserName { get; }
        string? FullName { get; }
        string? Stanowisko { get; }
        string? Rola { get; }

        bool IsLoggedIn { get; }

        bool IsAdmin { get; }
        bool IsPracownik { get; }
        bool IsTrener { get; }
        bool IsKlient { get; }

        void SetCurrentUser(
            int userId,
            string userName,
            string fullName,
            string? stanowisko,
            string? rola);

        void Clear();
        //do usuneicia
        void SetRole(string role);
    }
}