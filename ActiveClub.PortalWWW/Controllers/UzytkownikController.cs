using ActiveClub.PortalWWW.Services.Api;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.PortalWWW.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly CzlonkostwaUzytkownikaApiClient _czlonkostwaApiClient;
        private readonly UzytkownicyApiClient _uzytkownicyApiClient;
        private readonly RejestracjeNaZajeciaApiClient _rejestracjeNaZajeciaApiClient;
        private readonly RezerwacjeTreneraApiClient _rezerwacjeTreneraApiClient;
        private readonly KlubyApiClient _klubyApiClient;
        private readonly TrenerzyApiClient _trenerzyApiClient;

        public UzytkownikController(
     CzlonkostwaUzytkownikaApiClient czlonkostwaApiClient,
     RejestracjeNaZajeciaApiClient rejestracjeNaZajeciaApiClient,
     RezerwacjeTreneraApiClient rezerwacjeTreneraApiClient,
     KlubyApiClient klubyApiClient,
     TrenerzyApiClient trenerzyApiClient,
     UzytkownicyApiClient uzytkownicyApiClient)
        {
            _czlonkostwaApiClient = czlonkostwaApiClient;
            _uzytkownicyApiClient = uzytkownicyApiClient;
            _rezerwacjeTreneraApiClient = rezerwacjeTreneraApiClient;
            _rejestracjeNaZajeciaApiClient = rejestracjeNaZajeciaApiClient;
            _klubyApiClient = klubyApiClient;
            _trenerzyApiClient = trenerzyApiClient;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnulujKarnet(int czlonkostwoId)
        {
            var success =
                await _czlonkostwaApiClient.AnulujAsync(czlonkostwoId);

            TempData[success ? "Success" : "Error"] =
                success
                    ? "Karnet został anulowany."
                    : "Nie udało się anulować karnetu.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UmowTrening(int? klubId)
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrWhiteSpace(userIdText))
                return RedirectToAction("Logowanie", "Konto");

            var kluby = await _klubyApiClient.GetKlubyAsync();
            var trenerzy = await _trenerzyApiClient.GetAsync();

            if (klubId.HasValue)
            {
                trenerzy = trenerzy
                    .Where(t => t.KlubIds.Contains(klubId.Value))
                    .ToList();
            }

            ViewBag.Kluby = kluby;
            ViewBag.Trenerzy = trenerzy;
            ViewBag.KlubId = klubId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UmowTrening(
    int klubId,
    int trenerId,
    DateTime startAt)
        {
            Console.WriteLine($"TRENING: klub={klubId}, trener={trenerId}, start={startAt}");
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrWhiteSpace(userIdText))
                return RedirectToAction("Logowanie", "Konto");

            var userId = int.Parse(userIdText);

            var dto = new ActiveClub.Shared.DTOs.RezerwacjeTrenera.CreateRezerwacjaTreneraDto
            {
                KlubId = klubId,
                TrenerId = trenerId,
                UzytkownikId = userId,
                StartAt = startAt,
                EndAt = startAt.AddHours(1),
                Status = "NOWA",
                Cena = null
            };

            var success = await _rezerwacjeTreneraApiClient.CreateAsync(dto);

            TempData[success ? "Success" : "Error"] =
                success
                    ? "Umówiono trening personalny."
                    : "Nie udało się umówić treningu.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrWhiteSpace(userIdText))
                return RedirectToAction("Logowanie", "Konto");

            var userId = int.Parse(userIdText);

            var karnety = await _czlonkostwaApiClient.GetByUzytkownikAsync(userId);

            var user = await _uzytkownicyApiClient.GetByIdAsync(userId);

            var mojeZajecia =
                await _rejestracjeNaZajeciaApiClient.GetByUzytkownikIdAsync(userId);
            var mojeTreningi =
    await _rezerwacjeTreneraApiClient.GetByUzytkownikIdAsync(userId);

            ViewBag.Imie = user?.Imie ?? HttpContext.Session.GetString("Imie");
            ViewBag.Nazwisko = user?.Nazwisko ?? HttpContext.Session.GetString("Nazwisko");
            ViewBag.Email = user?.Email;
            ViewBag.Telefon = user?.Telefon;

            ViewBag.MojeZajecia = mojeZajecia;
            ViewBag.MojeTreningi = mojeTreningi;

            return View(karnety);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnulujZapisNaZajecia(int rejestracjaId)
        {
            await _rejestracjeNaZajeciaApiClient.UpdateStatusAsync(
                rejestracjaId,
                "ANULOWANY");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edytuj()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrWhiteSpace(userIdText))
                return RedirectToAction("Logowanie", "Konto");

            var userId = int.Parse(userIdText);

            var user = await _uzytkownicyApiClient.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            var model = new UpdateUzytkownikDto
            {
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                Email = user.Email,
                Telefon = user.Telefon,
                CzyAktywny = user.CzyAktywny,
                KlubDomyslnyId = user.KlubDomyslnyId,
                Uwagi = user.Uwagi,
                Rola = user.Rola ?? "Klient",
                KlubId = user.KlubId,
                StanowiskoId = user.StanowiskoId
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EdytujTrening(int id)
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrWhiteSpace(userIdText))
                return RedirectToAction("Logowanie", "Konto");

            var trening = await _rezerwacjeTreneraApiClient.GetByIdAsync(id);

            if (trening == null)
                return NotFound();

            return View(trening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(UpdateUzytkownikDto model)
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrWhiteSpace(userIdText))
                return RedirectToAction("Logowanie", "Konto");

            var userId = int.Parse(userIdText);

            var success = await _uzytkownicyApiClient.UpdateAsync(userId, model);

            if (!success)
            {
                ModelState.AddModelError("", "Nie udało się zaktualizować danych.");
                return View(model);
            }

            HttpContext.Session.SetString("Imie", model.Imie ?? "");
            HttpContext.Session.SetString("Nazwisko", model.Nazwisko ?? "");

            TempData["Sukces"] = "Dane zostały zaktualizowane.";

            return RedirectToAction(nameof(Index));
        }
    }
}