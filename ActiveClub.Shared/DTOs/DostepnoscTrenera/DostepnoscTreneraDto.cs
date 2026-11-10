namespace ActiveClub.Shared.DTOs.DostepnoscTrenera
{
    public class DostepnoscTreneraDto
    {
        public int DostepnoscId { get; set; }
        public int TrenerId { get; set; }
        public int? KlubId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string? Status { get; set; }
        public DateTime? UtworzonoAt { get; set; }
    }
}