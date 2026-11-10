using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Raporty;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ActiveClub.API.Services
{
    public class RaportyService : IRaportyService
    {
        private readonly ActiveClubContext _context;
        private readonly IRaportPdfService _raportPdfService;
        private readonly IWebHostEnvironment _environment;

        public RaportyService(
            ActiveClubContext context,
            IRaportPdfService raportPdfService,
            IWebHostEnvironment environment)
        {
            _context = context;
            _raportPdfService = raportPdfService;
            _environment = environment;
        }

        public async Task<RaportFormDataDto> GetFormDataAsync()
        {
            try
            {
                var dto = new RaportFormDataDto();

                dto.DefinicjeRaportow = await _context.RaportyDefinicjes
                    .AsNoTracking()
                    .Where(x => x.CzyAktywny)
                    .OrderBy(x => x.Nazwa)
                    .Select(x => new LookupItemDto
                    {
                        Id = x.RaportDefinicjaId,
                        Nazwa = x.Nazwa
                    })
                    .ToListAsync();

                dto.Kluby = await _context.Klubies
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwa)
                    .Select(x => new LookupItemDto
                    {
                        Id = x.KlubId,
                        Nazwa = x.Nazwa
                    })
                    .ToListAsync();

                dto.Trenerzy = await _context.Trenerzies
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwisko)
                    .ThenBy(x => x.Imie)
                    .Select(x => new LookupItemDto
                    {
                        Id = x.TrenerId,
                        Nazwa = (x.Imie + " " + x.Nazwisko).Trim()
                    })
                    .ToListAsync();

                dto.Formaty = new List<LookupItemDto>
                {
                    new LookupItemDto { Id = 1, Nazwa = "PDF" }
                };

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync Raporty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<RaportDefinicjaDto>> GetDefinitionsAsync()
        {
            try
            {
                return await _context.RaportyDefinicjes
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwa)
                    .Select(x => new RaportDefinicjaDto
                    {
                        RaportDefinicjaId = x.RaportDefinicjaId,
                        KodRaportu = x.KodRaportu,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis,
                        ParametryJson = x.ParametryJson,
                        CzyAktywny = x.CzyAktywny
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetDefinitionsAsync Raporty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<RaportWygenerowanyDto>> GetGeneratedAsync()
        {
            try
            {
                return await _context.RaportyWygenerowanes
                    .AsNoTracking()
                    .Include(x => x.RaportDefinicja)
                    .OrderByDescending(x => x.DataGeneracji)
                    .Select(x => new RaportWygenerowanyDto
                    {
                        RaportWygenerowanyId = x.RaportWygenerowanyId,
                        RaportDefinicjaId = x.RaportDefinicjaId,
                        NazwaRaportu = x.RaportDefinicja.Nazwa,
                        WygenerowalUzytkownikId = x.WygenerowalUzytkownikId,
                        Format = x.Format,
                        Status = x.Status,
                        PlikUrl = x.PlikUrl,
                        ParametryJson = x.ParametryJson,
                        DataGeneracji = x.DataGeneracji
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetGeneratedAsync Raporty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<GenerateRaportResultDto> GenerateAsync(GenerateRaportDto dto)
        {
            try
            {
                var definicja = await _context.RaportyDefinicjes
                    .FirstOrDefaultAsync(x => x.RaportDefinicjaId == dto.RaportDefinicjaId && x.CzyAktywny);

                if (definicja == null)
                {
                    return new GenerateRaportResultDto
                    {
                        Success = false,
                        Message = "Nie znaleziono aktywnej definicji raportu."
                    };
                }

                if (!string.Equals(dto.Format, "PDF", StringComparison.OrdinalIgnoreCase))
                {
                    return new GenerateRaportResultDto
                    {
                        Success = false,
                        Message = "Aktualnie obsługiwany jest tylko format PDF."
                    };
                }

                switch (definicja.KodRaportu)
                {
                    case "sales_summary":
                        return await GenerateSalesSummaryAsync(definicja, dto);

                    case "active_users":
                        return await GenerateActiveUsersAsync(definicja, dto);

                    case "active_trainers":
                        return await GenerateActiveTrainersAsync(definicja, dto);

                    case "clubs_summary":
                        return await GenerateClubsSummaryAsync(definicja, dto);

                    case "memberships_summary":
                        return await GenerateMembershipsSummaryAsync(definicja, dto);

                    default:
                        return new GenerateRaportResultDto
                        {
                            Success = false,
                            Message = $"Nieobsługiwany kod raportu: {definicja.KodRaportu}"
                        };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GenerateAsync Raporty:");
                Console.WriteLine(ex.ToString());

                return new GenerateRaportResultDto
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        private async Task<GenerateRaportResultDto> GenerateSalesSummaryAsync(
            RaportyDefinicje definicja,
            GenerateRaportDto dto)
        {
            var query = _context.Zamowienia
                .AsNoTracking()
                .AsQueryable();

            if (dto.DataOd.HasValue)
                query = query.Where(x => x.DataUtworzenia >= dto.DataOd.Value);

            if (dto.DataDo.HasValue)
                query = query.Where(x => x.DataUtworzenia <= dto.DataDo.Value);

            if (dto.KlubId.HasValue && dto.KlubId.Value > 0)
                query = query.Where(x => x.KlubId == dto.KlubId.Value);

            var orders = await query
                .OrderByDescending(x => x.DataUtworzenia)
                .ToListAsync();

            var totalOrders = orders.Count;
            var totalAmount = orders.Sum(x => x.SumaBrutto);
            var totalDiscount = orders.Sum(x => x.KwotaRabatu);
            var paidOrders = orders.Count(x => x.DataOplacenia != null);

            var generatedAt = DateTime.Now;

            var summary =
                $"{await BuildFilterSummaryAsync(dto)}\n" +
                $"Zamówienia: {totalOrders}\n" +
                $"Opłacone: {paidOrders}\n" +
                $"Suma: {totalAmount:N2} zł\n" +
                $"Rabaty: {totalDiscount:N2} zł";

            var items = orders
                .Select(x =>
                    $"{x.NumerZamowienia} | {x.Status} | {x.SumaBrutto:N2} {x.Waluta} | {x.DataUtworzenia:yyyy-MM-dd}")
                .ToList();

            var pdfBytes = _raportPdfService.GenerateSimplePdf(
                definicja.Nazwa,
                generatedAt,
                summary,
                items);

            return await SaveGeneratedReportAsync(definicja, dto, pdfBytes, generatedAt);
        }

        private async Task<GenerateRaportResultDto> GenerateActiveUsersAsync(
            RaportyDefinicje definicja,
            GenerateRaportDto dto)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var query = _context.CzlonkostwaUzytkownikas
                .AsNoTracking()
                .AsQueryable();

            if (dto.KlubId.HasValue && dto.KlubId.Value > 0)
                query = query.Where(x => x.KlubId == dto.KlubId.Value);

            if (dto.DataOd.HasValue)
                query = query.Where(x => x.DataUtworzenia.HasValue && x.DataUtworzenia.Value >= dto.DataOd.Value);

            if (dto.DataDo.HasValue)
                query = query.Where(x => x.DataUtworzenia.HasValue && x.DataUtworzenia.Value <= dto.DataDo.Value);

            var activeUserIds = await query
                .Where(x =>
                    x.StartAt.HasValue &&
                    x.StartAt.Value <= today &&
                    (!x.EndAt.HasValue || x.EndAt.Value >= today))
                .Select(x => x.UzytkownikId)
                .Distinct()
                .ToListAsync();

            var users = await _context.Users
                .AsNoTracking()
                .Where(x => activeUserIds.Contains(x.Id))
                .OrderBy(x => x.Nazwisko)
                .ThenBy(x => x.Imie)
                .Select(x => $"{x.Imie} {x.Nazwisko} | {x.Email}")
                .ToListAsync();

            var generatedAt = DateTime.Now;

            var summary =
                $"{await BuildFilterSummaryAsync(dto)}\n" +
                $"Liczba aktywnych użytkowników: {activeUserIds.Count}";

            var pdfBytes = _raportPdfService.GenerateSimplePdf(
                definicja.Nazwa,
                generatedAt,
                summary,
                users);

            return await SaveGeneratedReportAsync(definicja, dto, pdfBytes, generatedAt);
        }

        private async Task<GenerateRaportResultDto> GenerateActiveTrainersAsync(
            RaportyDefinicje definicja,
            GenerateRaportDto dto)
        {
            var query = _context.Trenerzies
                .AsNoTracking()
                .Where(x => x.CzyAktywny)
                .AsQueryable();

            if (dto.TrenerId.HasValue && dto.TrenerId.Value > 0)
                query = query.Where(x => x.TrenerId == dto.TrenerId.Value);

            if (dto.KlubId.HasValue && dto.KlubId.Value > 0)
            {
                query = query.Where(x =>
                    x.TrenerKlubs.Any(tk => tk.KlubId == dto.KlubId.Value));
            }

            var trainers = await query
                .OrderBy(x => x.Nazwisko)
                .ThenBy(x => x.Imie)
                .ToListAsync();

            var generatedAt = DateTime.Now;

            var summary =
                $"{await BuildFilterSummaryAsync(dto)}\n" +
                $"Aktywni trenerzy: {trainers.Count}";

            var items = trainers
                .Select(x =>
                    $"{x.Imie} {x.Nazwisko} | Stawka: {(x.StawkaZaGodzine.HasValue ? x.StawkaZaGodzine.Value.ToString("N2") : "brak")}")
                .ToList();

            var pdfBytes = _raportPdfService.GenerateSimplePdf(
                definicja.Nazwa,
                generatedAt,
                summary,
                items);

            return await SaveGeneratedReportAsync(definicja, dto, pdfBytes, generatedAt);
        }

        private async Task<GenerateRaportResultDto> GenerateClubsSummaryAsync(
            RaportyDefinicje definicja,
            GenerateRaportDto dto)
        {
            var query = _context.Klubies
                .AsNoTracking()
                .AsQueryable();

            if (dto.KlubId.HasValue && dto.KlubId.Value > 0)
                query = query.Where(x => x.KlubId == dto.KlubId.Value);

            var clubs = await query
                .OrderBy(x => x.Nazwa)
                .ToListAsync();

            var active = clubs.Count(x => x.CzyAktywny);

            var generatedAt = DateTime.Now;

            var summary =
                $"{await BuildFilterSummaryAsync(dto)}\n" +
                $"Liczba klubów: {clubs.Count}\n" +
                $"Aktywne: {active}";

            var items = clubs
                .Select(x => string.IsNullOrWhiteSpace(x.Miasto)
                    ? x.Nazwa
                    : $"{x.Nazwa} ({x.Miasto})")
                .ToList();

            var pdfBytes = _raportPdfService.GenerateSimplePdf(
                definicja.Nazwa,
                generatedAt,
                summary,
                items);

            return await SaveGeneratedReportAsync(definicja, dto, pdfBytes, generatedAt);
        }

        private async Task<GenerateRaportResultDto> GenerateMembershipsSummaryAsync(
            RaportyDefinicje definicja,
            GenerateRaportDto dto)
        {
            var query = _context.CzlonkostwaUzytkownikas
                .AsNoTracking()
                .AsQueryable();

            if (dto.DataOd.HasValue)
                query = query.Where(x => x.DataUtworzenia.HasValue && x.DataUtworzenia.Value >= dto.DataOd.Value);

            if (dto.DataDo.HasValue)
                query = query.Where(x => x.DataUtworzenia.HasValue && x.DataUtworzenia.Value <= dto.DataDo.Value);

            if (dto.KlubId.HasValue && dto.KlubId.Value > 0)
                query = query.Where(x => x.KlubId == dto.KlubId.Value);

            var memberships = await query
                .OrderByDescending(x => x.DataUtworzenia)
                .ToListAsync();

            var total = memberships.Count;

            var active = memberships.Count(x =>
                !string.IsNullOrWhiteSpace(x.Status) &&
                x.Status.Trim().ToLower() == "aktywne");

            var revenue = memberships.Sum(x => x.CenaZakupu ?? 0m);

            var generatedAt = DateTime.Now;

            var summary =
                $"{await BuildFilterSummaryAsync(dto)}\n" +
                $"Liczba członkostw: {total}\n" +
                $"Aktywne: {active}\n" +
                $"Przychód: {revenue:N2} zł";

            var items = memberships
                .Select(x =>
                    $"Użytkownik ID: {x.UzytkownikId} | Status: {x.Status} | Cena: {(x.CenaZakupu ?? 0m):N2} | Klub ID: {x.KlubId}")
                .ToList();

            var pdfBytes = _raportPdfService.GenerateSimplePdf(
                definicja.Nazwa,
                generatedAt,
                summary,
                items);

            return await SaveGeneratedReportAsync(definicja, dto, pdfBytes, generatedAt);
        }

        private async Task<string> BuildFilterSummaryAsync(GenerateRaportDto dto)
        {
            var filters = new List<string>();

            if (dto.DataOd.HasValue)
                filters.Add($"Data od: {dto.DataOd.Value:yyyy-MM-dd}");

            if (dto.DataDo.HasValue)
                filters.Add($"Data do: {dto.DataDo.Value:yyyy-MM-dd}");

            if (dto.KlubId.HasValue && dto.KlubId.Value > 0)
            {
                var klub = await _context.Klubies
                    .AsNoTracking()
                    .Where(x => x.KlubId == dto.KlubId.Value)
                    .Select(x => x.Nazwa)
                    .FirstOrDefaultAsync();

                filters.Add($"Klub: {klub ?? dto.KlubId.Value.ToString()}");
            }

            if (dto.TrenerId.HasValue && dto.TrenerId.Value > 0)
            {
                var trener = await _context.Trenerzies
                    .AsNoTracking()
                    .Where(x => x.TrenerId == dto.TrenerId.Value)
                    .Select(x => (x.Imie + " " + x.Nazwisko).Trim())
                    .FirstOrDefaultAsync();

                filters.Add($"Trener: {trener ?? dto.TrenerId.Value.ToString()}");
            }

            if (!filters.Any())
                return "Filtry: brak";

            return "Filtry:\n" + string.Join("\n", filters);
        }

        private async Task<GenerateRaportResultDto> SaveGeneratedReportAsync(
            RaportyDefinicje definicja,
            GenerateRaportDto dto,
            byte[] pdfBytes,
            DateTime generatedAt)
        {
            var reportsFolder = Path.Combine(_environment.ContentRootPath, "GeneratedReports");
            Directory.CreateDirectory(reportsFolder);

            var safeFileName = $"raport-{definicja.KodRaportu}-{generatedAt:yyyyMMdd-HHmmss}.pdf";
            var fullPath = Path.Combine(reportsFolder, safeFileName);

            await File.WriteAllBytesAsync(fullPath, pdfBytes);

            var parametry = new
            {
                dto.DataOd,
                dto.DataDo,
                dto.KlubId,
                dto.TrenerId
            };

            var entity = new RaportyWygenerowane
            {
                RaportDefinicjaId = definicja.RaportDefinicjaId,
                WygenerowalUzytkownikId = dto.WygenerowalUzytkownikId,
                Format = "PDF",
                Status = "Gotowy",
                PlikUrl = fullPath,
                ParametryJson = JsonSerializer.Serialize(parametry),
                DataGeneracji = generatedAt
            };

            _context.RaportyWygenerowanes.Add(entity);
            await _context.SaveChangesAsync();

            return new GenerateRaportResultDto
            {
                Success = true,
                RaportWygenerowanyId = entity.RaportWygenerowanyId,
                Message = "Raport PDF został wygenerowany.",
                PlikUrl = fullPath
            };
        }
    }
}