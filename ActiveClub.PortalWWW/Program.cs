using ActiveClub.PortalWWW.Services;
using ActiveClub.PortalWWW.Services.Api;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Konto/Logowanie";
        options.LogoutPath = "/Konto/Wyloguj";

        options.ExpireTimeSpan = TimeSpan.FromDays(30);

        options.SlidingExpiration = true;
    });

builder.Services.AddHttpClient("ActiveClubApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5006/");
});

builder.Services.AddScoped<ZajeciaApiClient>();
builder.Services.AddScoped<AktualnosciApiClient>();
builder.Services.AddScoped<UdogodnieniaApiClient>();
builder.Services.AddScoped<RezerwacjeTreneraApiClient>();
builder.Services.AddScoped<KlubyApiClient>();
builder.Services.AddScoped<UzytkownicyApiClient>();
builder.Services.AddScoped<RodzajeCzlonkostwaApiClient>();
builder.Services.AddScoped<HarmonogramApiClient>();
builder.Services.AddScoped<TrenerzyApiClient>();
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<CzlonkostwaUzytkownikaApiClient>();
builder.Services.AddScoped<KontaktApiClient>();
builder.Services.AddScoped<RejestracjeNaZajeciaApiClient>();
builder.Services.AddScoped<UstawieniaKontaktoweApiClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();