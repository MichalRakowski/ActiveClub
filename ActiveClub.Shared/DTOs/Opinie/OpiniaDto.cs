namespace ActiveClub.Shared.DTOs.Opinie
{
    public class OpiniaDto
    {
        public int OpiniaId { get; set; }
        public int KlubId { get; set; }
        public int UzytkownikId { get; set; }
        public byte Ocena { get; set; }
        public string? Komentarz { get; set; }
        public bool CzyWidoczna { get; set; }
        public DateTime DataDodania { get; set; }
    }
}