using System.Security.Claims;
using ActiveClub.PortalWWW.Models;
using ActiveClub.PortalWWW.Services.Api;
using ActiveClub.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.PortalWWW.Controllers
{
    public class KontoController : Controller
    {
        private readonly AuthApiClient _authApiClient;

        public KontoController(AuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        [HttpGet]
        public IActionResult Rejestracja()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rejestracja(RejestracjaViewModel model)
        {
            var dto = new RegisterDto
            {
                Imie = model.Imie,
                Nazwisko = model.Nazwisko,
                Email = model.Email,
                Telefon = model.Telefon,
                Haslo = model.Haslo,
                PowtorzHaslo = model.PowtorzHaslo,
                AkceptujeRegulamin = model.AkceptujeRegulamin
            };

            var result = await _authApiClient.RegisterAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage ?? "Wystąpił błąd podczas rejestracji.");

                return View(model);
            }

            TempData["SuccessMessage"] = "Konto zostało utworzone.";
            return RedirectToAction(nameof(Logowanie));
        }

        [HttpGet]
        public IActionResult Logowanie(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logowanie(LoginViewModel model, string? returnUrl = null)
        {
            var dto = new LoginDto
            {
                Login = model.Email,
                Haslo = model.Password
            };

            var result = await _authApiClient.LoginAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage ?? "Nieprawidłowy login lub hasło.");

                return View(model);
            }

            HttpContext.Session.SetString("Token", result.Token ?? "");
            HttpContext.Session.SetString("UserId", result.UserId.ToString());
            HttpContext.Session.SetString("Imie", result.Imie ?? "");
            HttpContext.Session.SetString("Nazwisko", result.Nazwisko ?? "");
            HttpContext.Session.SetString("RememberMe", model.RememberMe ? "true" : "false");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{result.Imie} {result.Nazwisko}".Trim()),
                new Claim(ClaimTypes.Email, model.Email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            var klubId = HttpContext.Session.GetInt32("WybranyKlubId");
            var rodzajId = HttpContext.Session.GetInt32("WybranyRodzajId");

            if (klubId.HasValue && rodzajId.HasValue)
            {
                return RedirectToAction("Platnosc", "Karnety", new
                {
                    klubId = klubId.Value,
                    rodzajId = rodzajId.Value
                });
            }

            return RedirectToAction("Index", "Uzytkownik");
        }

        [HttpGet]
        public async Task<IActionResult> Wyloguj()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Logowanie));
        }

        [HttpGet]
        public IActionResult ZapomnialesHasla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ZapomnialesHasla(string email)
        {
            TempData["Info"] = "Jeżeli konto istnieje, wysłaliśmy link do resetu hasła.";
            return RedirectToAction(nameof(ZapomnialesHasla));
        }
    }
}