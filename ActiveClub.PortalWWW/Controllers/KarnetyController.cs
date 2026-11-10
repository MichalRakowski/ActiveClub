using ActiveClub.PortalWWW.Services.Api;
using Microsoft.AspNetCore.Mvc;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;

namespace ActiveClub.PortalWWW.Controllers;

public class KarnetyController : Controller
{
    private readonly KlubyApiClient _klubyApiClient;
    private readonly RodzajeCzlonkostwaApiClient _rodzajeCzlonkostwaApiClient;
    private readonly CzlonkostwaUzytkownikaApiClient _czlonkostwaApiClient;

    public KarnetyController(
        KlubyApiClient klubyApiClient,
        RodzajeCzlonkostwaApiClient rodzajeCzlonkostwaApiClient,
        CzlonkostwaUzytkownikaApiClient czlonkostwaApiClient)
    {
        _klubyApiClient = klubyApiClient;
        _rodzajeCzlonkostwaApiClient = rodzajeCzlonkostwaApiClient;
        _czlonkostwaApiClient = czlonkostwaApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> KupKarnet(string? q, string? miasto)
    {
        ViewBag.Miasta = await _klubyApiClient.GetMiastaAsync();

        var kluby = await _klubyApiClient.GetKlubyAsync(q, miasto);
        return View("KupKarnet", kluby);
    }

    [HttpGet("/Karnety/WybierzKarnet/{klubId:int}")]
    public async Task<IActionResult> WybierzKarnet(int klubId)
    {
        var klub = await _klubyApiClient.GetKlubAsync(klubId);

        if (klub == null)
            return NotFound();

        var rodzaje = await _rodzajeCzlonkostwaApiClient.GetAsync();

        var dostepneRodzaje = rodzaje
            .Where(r => r.CzyAktywny && r.CzyWidoczneOnline)
            .OrderBy(r => r.Nazwa)
            .ToList();

        ViewBag.Klub = klub;
        return View("WybierzKarnet", dostepneRodzaje);
    }

    [HttpGet]
    public async Task<IActionResult> PodsumowanieZakupu(int klubId, int rodzajId)
    {
        HttpContext.Session.SetInt32("WybranyKlubId", klubId);
        HttpContext.Session.SetInt32("WybranyRodzajId", rodzajId);

        var klub = await _klubyApiClient.GetKlubAsync(klubId);
        if (klub == null)
            return NotFound();

        var rodzaje = await _rodzajeCzlonkostwaApiClient.GetAsync();

        var rodzaj = rodzaje.FirstOrDefault(r =>
            r.RodzajCzlonkostwaId == rodzajId &&
            r.CzyAktywny &&
            r.CzyWidoczneOnline);

        if (rodzaj == null)
            return NotFound();

        ViewBag.Klub = klub;
        return View("PodsumowanieZakupu", rodzaj);
    }

    [HttpGet]
    public async Task<IActionResult> Platnosc(int klubId, int rodzajId)
    {
        var klub = await _klubyApiClient.GetKlubAsync(klubId);
        if (klub == null)
            return NotFound();

        var rodzaje = await _rodzajeCzlonkostwaApiClient.GetAsync();

        var rodzaj = rodzaje.FirstOrDefault(r =>
            r.RodzajCzlonkostwaId == rodzajId &&
            r.CzyAktywny &&
            r.CzyWidoczneOnline);

        if (rodzaj == null)
            return NotFound();

        ViewBag.Klub = klub;

        return View("Platnosc", rodzaj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FinalizujPlatnosc(int klubId, int rodzajId)
    {
        var userIdText = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrWhiteSpace(userIdText))
        {
            return RedirectToAction("Logowanie", "Konto");
        }

        var userId = int.Parse(userIdText);

        var zapisano = await _czlonkostwaApiClient.ZakupAsync(new ZakupKarnetuDto
        {
            UzytkownikId = userId,
            KlubId = klubId,
            RodzajCzlonkostwaId = rodzajId
        });

        if (!zapisano)
        {
            TempData["BladPlatnosci"] = "Nie udało się zapisać karnetu.";
            return RedirectToAction("Platnosc", new { klubId, rodzajId });
        }

        TempData["SukcesPlatnosci"] = "Płatność została zrealizowana pomyślnie.";

        return RedirectToAction("Potwierdzenie");
    }

    [HttpGet]
    public IActionResult Potwierdzenie()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ZakupPotwierdz(int klubId, int rodzajId)
    {
        if (!(User?.Identity?.IsAuthenticated ?? false))
        {
            return RedirectToAction("Logowanie", "Konto",
                new { returnUrl = Url.Action("PodsumowanieZakupu", "Karnety", new { klubId, rodzajId }) });
        }

        var klub = await _klubyApiClient.GetKlubAsync(klubId);
        if (klub == null)
            return NotFound();

        var rodzaje = await _rodzajeCzlonkostwaApiClient.GetAsync();
        var rodzaj = rodzaje.FirstOrDefault(r =>
            r.RodzajCzlonkostwaId == rodzajId &&
            r.CzyAktywny &&
            r.CzyWidoczneOnline);

        if (rodzaj == null)
            return NotFound();

        TempData["Sukces"] = $"Zakup karnetu \"{rodzaj.Nazwa}\" został zapisany (demo).";
        return RedirectToAction("Index", "Home");
    }
}