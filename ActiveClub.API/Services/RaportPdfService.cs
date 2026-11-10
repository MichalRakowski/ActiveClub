using ActiveClub.API.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ActiveClub.API.Services
{
    public class RaportPdfService : IRaportPdfService
    {
        public byte[] GenerateSimplePdf(
            string title,
            DateTime generatedAt,
            string summary,
            List<string> items)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Column(col =>
                    {
                        col.Item().Text(title).FontSize(20).Bold();
                        col.Item().Text($"Data wygenerowania: {generatedAt:yyyy-MM-dd HH:mm}");
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(15).Text(summary);

                        if (items != null && items.Any())
                        {
                            foreach (var item in items)
                                col.Item().Text($"• {item}");
                        }
                    });

                    page.Footer().AlignCenter().Text("ActiveClub - raport PDF");
                });
            }).GeneratePdf();
        }
    }
}