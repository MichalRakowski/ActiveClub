using ActiveClub.Firma.Services;
using ActiveClub.Firma.ViewModels;
using ActiveClub.Firma.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace ActiveClub.Firma
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; } = null!;

        public App()
        {
            var builder = Host.CreateApplicationBuilder();
            var apiBaseUrl = new Uri("https://localhost:7134/");

            ConfigureLogging(builder);
            ConfigureCoreServices(builder.Services);
            ConfigureApiClients(builder.Services, apiBaseUrl);
            ConfigureWindowsAndViewModels(builder.Services);

            AppHost = builder.Build();
        }

        private static void ConfigureLogging(HostApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddDebug();
        }

        private static void ConfigureCoreServices(IServiceCollection services)
        {
            services.AddTransient<AuthMessageHandler>();
            services.AddSingleton<UserSession>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
        }

        private static void ConfigureApiClients(IServiceCollection services, Uri apiBaseUrl)
        {
            AddApiClientWithoutAuth<AuthService>(services, apiBaseUrl);

            AddApiClient<StanowiskaService>(services, apiBaseUrl);
            AddApiClient<RaportyService>(services, apiBaseUrl);
            AddApiClient<CzlonkostwaUzytkownikaService>(services, apiBaseUrl);
            AddApiClient<DashboardService>(services, apiBaseUrl);
            AddApiClient<RejestracjeNaZajeciaService>(services, apiBaseUrl);
            AddApiClient<OfertyKlubuCzlonkostwaService>(services, apiBaseUrl);
            AddApiClient<PozycjeZamowieniaService>(services, apiBaseUrl);
            AddApiClient<PracownicyKlubuService>(services, apiBaseUrl);
            AddApiClient<UzytkownicyService>(services, apiBaseUrl);
            AddApiClient<UstawieniaKontaktoweService>(services, apiBaseUrl);
            AddApiClient<PlatnosciService>(services, apiBaseUrl);
            AddApiClient<AktualnosciService>(services, apiBaseUrl);
            AddApiClient<KlubyService>(services, apiBaseUrl);
            AddApiClient<ZamowieniaService>(services, apiBaseUrl);
            AddApiClient<RodzajeCzlonkostwaService>(services, apiBaseUrl);
            AddApiClient<HarmonogramZajecService>(services, apiBaseUrl);
            AddApiClient<TrenerzyService>(services, apiBaseUrl);
            AddApiClient<DostepnoscTreneraService>(services, apiBaseUrl);
            AddApiClient<RezerwacjeTreneraService>(services, apiBaseUrl);
            AddApiClient<OpinieService>(services, apiBaseUrl);
            AddApiClient<GodzinyOtwarciaService>(services, apiBaseUrl);
            AddApiClient<MetodyPlatnosciService>(services, apiBaseUrl);
            AddApiClient<SaleService>(services, apiBaseUrl);
            AddApiClient<RodzajeZajecService>(services, apiBaseUrl);
            AddApiClient<RabatyService>(services, apiBaseUrl);
        }

        private static void ConfigureWindowsAndViewModels(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<LoginWindow>();
            services.AddTransient<LoginViewModel>();
        }

        private static void AddApiClient<TService>(IServiceCollection services, Uri baseAddress)
            where TService : class
        {
            services.AddHttpClient<TService>(client =>
            {
                client.BaseAddress = baseAddress;
            })
            .AddHttpMessageHandler<AuthMessageHandler>();
        }

        private static void AddApiClientWithoutAuth<TService>(IServiceCollection services, Uri baseAddress)
            where TService : class
        {
            services.AddHttpClient<TService>(client =>
            {
                client.BaseAddress = baseAddress;
            });
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                await AppHost.StartAsync();

                var loginWindow = AppHost.Services.GetRequiredService<LoginWindow>();
                loginWindow.DataContext = AppHost.Services.GetRequiredService<LoginViewModel>();
                loginWindow.Show();

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się uruchomić aplikacji.\n\n{ex.Message}",
                    "Błąd uruchamiania",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Shutdown();
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                if (AppHost is not null)
                {
                    await AppHost.StopAsync();
                    AppHost.Dispose();
                }
            }
            finally
            {
                base.OnExit(e);
            }
        }
    }
}