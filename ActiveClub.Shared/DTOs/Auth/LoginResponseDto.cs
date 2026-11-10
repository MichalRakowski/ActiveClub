namespace ActiveClub.Shared.DTOs.Auth
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public string? Stanowisko { get; set; }
        public string? Rola { get; set; }
        public string? Token { get; set; }
    }
}