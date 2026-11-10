namespace ActiveClub.Shared.DTOs.Raporty
{
    public class GenerateRaportResultDto
    {
        public bool Success { get; set; }
        public int? RaportWygenerowanyId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? PlikUrl { get; set; }
    }
}