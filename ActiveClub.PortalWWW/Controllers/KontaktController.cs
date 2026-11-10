using ActiveClub.PortalWWW.Services.Api;
using ActiveClub.Shared.DTOs.Kontakt;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.PortalWWW.Controllers
{
    public class KontaktController : Controller
    {
        private readonly KontaktApiClient _apiClient;

        public KontaktController(KontaktApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(KontaktFormDto model)
        {
            var success = await _apiClient.SendAsync(model);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Nie udało się wysłać wiadomości.");

                return View(model);
            }

            TempData["Success"] =
                "Wiadomość została wysłana.";

            return RedirectToAction(nameof(Index));
        }
    }
}