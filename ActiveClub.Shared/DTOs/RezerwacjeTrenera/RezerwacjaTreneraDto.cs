namespace ActiveClub.Shared.DTOs.RezerwacjeTrenera
{
    public class RezerwacjaTreneraDto
    {
        public int RezerwacjaId { get; set; }
        public int TrenerId { get; set; }
        public int UzytkownikId { get; set; }
        public int? KlubId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string? Status { get; set; }
        public decimal? Cena { get; set; }
        public DateTime? DataUtworzenia { get; set; }
        public string TrenerNazwa { get; set; } = string.Empty;

        public string KlubNazwa { get; set; } = string.Empty;

    }
}