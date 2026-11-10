namespace ActiveClub.API.Services.Interfaces
{
    public interface IRaportPdfService
    {
        byte[] GenerateSimplePdf(
            string title,
            DateTime generatedAt,
            string summary,
            List<string> items);
    }
}