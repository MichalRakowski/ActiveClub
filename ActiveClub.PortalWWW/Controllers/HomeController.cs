using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActiveClub.PortalWWW.Services.Api;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using ActiveClub.Shared.DTOs.Udogodnienia;
using System.Security.Claims;
using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;
using ActiveClub.PortalWWW.Models;
using ActiveClub.Shared.DTOs.Kontakt;

namespace ActiveClub.PortalWWW.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AktualnosciApiClient _aktualnosciApiClient;
    private readonly KlubyApiClient _klubyApiClient;
    private readonly UzytkownicyApiClient _uzytkownicyApiClient;
    private readonly ZajeciaApiClient _zajeciaApiClient;
    private readonly RejestracjeNaZajeciaApiClient _rejestracjeNaZajeciaApiClient;
    private readonly HarmonogramApiClient _harmonogramApiClient;
    private readonly TrenerzyApiClient _trenerzyApiClient;
    private readonly RodzajeCzlonkostwaApiClient _rodzajeCzlonkostwaApiClient;
    private readonly KontaktApiClient _kontaktApiClient;
    private readonly UstawieniaKontaktoweApiClient _ustawieniaKontaktoweApiClient;

    public HomeController(
        ILogger<HomeController> logger,
        AktualnosciApiClient aktualnosciApiClient,
        KlubyApiClient klubyApiClient,
        RejestracjeNaZajeciaApiClient rejestracjeNaZajeciaApiClient,
        UzytkownicyApiClient uzytkownicyApiClient,
        ZajeciaApiClient zajeciaApiClient,
        KontaktApiClient kontaktApiClient,
        HarmonogramApiClient harmonogramApiClient,
        TrenerzyApiClient trenerzyApiClient,
       UstawieniaKontaktoweApiClient ustawieniaKontaktoweApiClient,
        RodzajeCzlonkostwaApiClient rodzajeCzlonkostwaApiClient)
    {
        _logger = logger;
        _aktualnosciApiClient = aktualnosciApiClient;
        _klubyApiClient = klubyApiClient;
        _kontaktApiClient = kontaktApiClient;
        _rejestracjeNaZajeciaApiClient = rejestracjeNaZajeciaApiClient;
        _uzytkownicyApiClient = uzytkownicyApiClient;
        _zajeciaApiClient = zajeciaApiClient;
        _harmonogramApiClient = harmonogramApiClient;
        _ustawieniaKontaktoweApiClient = ustawieniaKontaktoweApiClient;
        _trenerzyApiClient = trenerzyApiClient;
        _rodzajeCzlonkostwaApiClient = rodzajeCzlonkostwaApiClient;
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DolaczDoZajec(int harmonogramId)
    {
        if (User?.Identity?.IsAuthenticated != true)
            return RedirectToAction("Logowanie", "Konto");

        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("UserId")
            ?? User.FindFirstValue("sub");

        if (!int.TryParse(userIdValue, out var userId))
        {
            TempData["Error"] = "Nie udało się odczytać użytkownika.";
            return RedirectToAction(nameof(GrafikZajec));
        }

        var dto = new CreateRejestracjaNaZajeciaDto
        {
            HarmonogramId = harmonogramId,
            UzytkownikId = userId,
            Status = "ZAPISANY",
            DataRejestracji = DateTime.Now,
            Zrodlo = "WWW"
        };

        var success = await _rejestracjeNaZajeciaApiClient.CreateAsync(dto);

        TempData[success ? "Success" : "Error"] =
            success
                ? "Zapisano Cię na zajęcia."
                : "Nie udało się zapisać na zajęcia.";

        return RedirectToAction("Index", "Uzytkownik");
    }
    public async Task<IActionResult> Index()
    {
        var aktualnosci = await _aktualnosciApiClient.GetAktualnosciAsync();

        var widoczneAktualnosci = aktualnosci
            .Where(x => x.CzyWidoczna == true)
            .OrderByDescending(x => x.DataPublikacji ?? x.DataDodania)
            .Take(3)
            .ToList();

        var trenerzy = await _trenerzyApiClient.GetAsync();

        ViewBag.ModelAktualnosci = widoczneAktualnosci;
        ViewBag.Trenerzy = trenerzy;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Kontakt()
    {
        ViewBag.UstawieniaKontaktowe =
            await _ustawieniaKontaktoweApiClient.GetAsync();

        return View();
    }



    [HttpGet]
    public IActionResult PlanyTrening()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Kontakt(KontaktFormDto model)
    {
        var success = await _kontaktApiClient.SendAsync(model);

        if (!success)
        {
            ViewBag.UstawieniaKontaktowe =
                await _ustawieniaKontaktoweApiClient.GetAsync();

            ModelState.AddModelError("", "Nie udało się wysłać wiadomości.");

            return View(model);
        }

        TempData["Success"] =
            "Wiadomość została wysłana.";

        return RedirectToAction(nameof(Kontakt));
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult MedicoverSport()
    {
        return View();
    }
    public async Task<IActionResult> Aktualnosci()
    {
        var aktualnosci = await _aktualnosciApiClient.GetAktualnosciAsync();

        var widoczneAktualnosci = aktualnosci
            .Where(x => x.CzyWidoczna == true)
            .OrderByDescending(x => x.DataPublikacji ?? x.DataDodania)
            .ToList();

        return View(widoczneAktualnosci);
    }

    public IActionResult Onas()
    {
        return View();
    }

    public async Task<IActionResult> Kluby(string? q, string? miasto)
    {
        ViewBag.Miasta = await _klubyApiClient.GetMiastaAsync();

        var wyniki = await _klubyApiClient.GetKlubyAsync(q, miasto);
        return View(wyniki);
    }

    public async Task<IActionResult> Klub(int id)
    {
        var klub = await _klubyApiClient.GetKlubAsync(id);

        if (klub == null)
            return NotFound();

        return View(klub);
    }

    public async Task<IActionResult> Zajecia()
    {
        var zajecia = await _zajeciaApiClient.GetZajeciaAsync();
        return View(zajecia);
    }

    public async Task<IActionResult> Czlonkostwa()
    {
        var czlonkostwa = await _rodzajeCzlonkostwaApiClient.GetAsync();

        var widoczne = czlonkostwa
            .Where(x => x.CzyAktywny && x.CzyWidoczneOnline)
            .OrderBy(x => x.Nazwa)
            .ToList();

        return View(widoczne);
    }

    public IActionResult JakZaczac()
    {
        return View();
    }
    public IActionResult Regulamin()
    {
        return View();
    }
    public IActionResult Opinie()
    {
        return View();
    }
    public IActionResult Multisport()
    {
        return View();
    }
    public IActionResult PzuSport()
    {
        return View();
    }
    public IActionResult FitProfit()
    {
        return View();
    }
    public async Task<IActionResult> GrafikZajec(int? klubId, DateTime? dzien)
    {
        var wybranyDzien = dzien?.Date ?? DateTime.Today;

        var grafik = await _harmonogramApiClient.GetAsync(klubId, wybranyDzien);
        var kluby = await _klubyApiClient.GetKlubyAsync();

        ViewBag.Kluby = kluby;
        ViewBag.KlubId = klubId;
        ViewBag.Dzien = wybranyDzien;

        return View(grafik);
    }

    public async Task<IActionResult> Trenerzy()
    {
        var trenerzy = await _trenerzyApiClient.GetAsync();
        return View(trenerzy);
    }
    public IActionResult PlanyTreningowe()
    {
        ViewData["Title"] = "Plany treningowe";
        return View();
    }
    public IActionResult RODO()
    {
        ViewData["Title"] = "RODO";
        return View();
    }
    public IActionResult Udogodnienia()
    {
        var udogodnienia = new List<UdogodnienieDto>
{
    new UdogodnienieDto
    {
        UdogodnienieId = 1,
        Nazwa = "Nowoczesna strefa cardio",
        Opis = "Bieżnie, rowerki, orbitreki i sprzęt najwyższej jakości dostępny dla każdego klubowicza."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 2,
        Nazwa = "Strefa wolnych ciężarów",
        Opis = "Profesjonalna przestrzeń do treningu siłowego z hantlami, sztangami i ławkami."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 3,
        Nazwa = "Zajęcia grupowe",
        Opis = "Bogata oferta zajęć: fitness, stretching, zdrowy kręgosłup, trening funkcjonalny i wiele innych."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 4,
        Nazwa = "Trener personalny",
        Opis = "Indywidualne plany treningowe dopasowane do Twoich celów i możliwości."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 5,
        Nazwa = "Strefa funkcjonalna",
        Opis = "TRX, kettlebells, battle rope i sprzęt do treningu całego ciała."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 6,
        Nazwa = "Sauna",
        Opis = "Idealne miejsce na regenerację i relaks po intensywnym treningu."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 7,
        Nazwa = "Strefa relaksu",
        Opis = "Wygodna przestrzeń do odpoczynku po treningu."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 8,
        Nazwa = "Szatnie i prysznice",
        Opis = "Nowoczesne i czyste zaplecze sanitarne dla Twojego komfortu."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 9,
        Nazwa = "Klimatyzacja",
        Opis = "Komfortowe warunki treningowe przez cały rok."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 10,
        Nazwa = "Darmowe WiFi",
        Opis = "Bezpłatny dostęp do internetu na terenie całego klubu."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 11,
        Nazwa = "Bezpłatny parking",
        Opis = "Wygodne miejsca parkingowe dostępne dla klientów."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 12,
        Nazwa = "Dostęp 24/7",
        Opis = "Trenuj kiedy chcesz – klub otwarty całą dobę, 7 dni w tygodniu."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 13,
        Nazwa = "Aplikacja mobilna",
        Opis = "Rezerwuj zajęcia, sprawdzaj grafik i zarządzaj swoim kontem."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 14,
        Nazwa = "Bar fitness",
        Opis = "Zdrowe przekąski, shake’i białkowe i napoje energetyczne."
    },
    new UdogodnienieDto
    {
        UdogodnienieId = 15,
        Nazwa = "Analiza składu ciała",
        Opis = "Monitoruj swoje postępy dzięki profesjonalnym pomiarom."
    }
};

        return View(udogodnienia);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}