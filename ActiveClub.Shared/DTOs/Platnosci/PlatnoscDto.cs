namespace ActiveClub.Shared.DTOs.Platnosci
{
    public class PlatnoscDto
    {
        public int PlatnoscId { get; set; }
        public int ZamowienieId { get; set; }
        public int MetodaPlatnosciId { get; set; }
        public decimal Kwota { get; set; }
        public string Waluta { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string ProviderTransactionId { get; set; } = string.Empty;
        public DateTime? DataPlatnosci { get; set; }
        public DateTime? DataUtworzenia { get; set; }
        public string Uwagi { get; set; } = string.Empty;

        // pomocniczo do listy / combo
        public string? ZamowienieDisplay { get; set; }
        public string? MetodaPlatnosciDisplay { get; set; }
    }
}