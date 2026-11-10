using System.Text;
using ActiveClub.API.Configuration;
using ActiveClub.API.Seed;
using ActiveClub.API.Services;
using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.JsonConverters;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON converters
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
    });

// DbContext
builder.Services.AddDbContext<ActiveClubContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ActiveClubContext>()
    .AddDefaultTokenProviders();

// JWT
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("Brak konfiguracji JwtSettings.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ActiveClub API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Wpisz token JWT w formacie: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Services
builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<IPozycjeZamowieniaService, PozycjeZamowieniaService>();
builder.Services.AddScoped<IZajeciaService, ZajeciaService>();
builder.Services.AddScoped<ICzlonkostwaUzytkownikaService, CzlonkostwaUzytkownikaService>();
builder.Services.AddScoped<IGodzinyOtwarciaService, GodzinyOtwarciaService>();
builder.Services.AddScoped<IOfertyKlubuCzlonkostwaService, OfertyKlubuCzlonkostwaService>();
builder.Services.AddScoped<IAktualnosciService, AktualnosciService>();
builder.Services.AddScoped<IPlatnosciService, PlatnosciService>();
builder.Services.AddScoped<IRejestracjeNaZajeciaService, RejestracjeNaZajeciaService>();
builder.Services.AddScoped<IHarmonogramZajecService, HarmonogramZajecService>();
builder.Services.AddScoped<IKlubyService, KlubyService>();
builder.Services.AddScoped<IPracownicyKlubuService, PracownicyKlubuService>();
builder.Services.AddScoped<IRodzajeCzlonkostwaService, RodzajeCzlonkostwaService>();
builder.Services.AddScoped<ITrenerzyService, TrenerzyService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUzytkownicyService, UzytkownicyService>();
builder.Services.AddScoped<IRezerwacjeTreneraService, RezerwacjeTreneraService>();
builder.Services.AddScoped<IDostepnoscTreneraService, DostepnoscTreneraService>();
builder.Services.AddScoped<IMetodyPlatnosciService, MetodyPlatnosciService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IRodzajeZajecService, RodzajeZajecService>();
builder.Services.AddScoped<IRabatyService, RabatyService>();
builder.Services.AddScoped<IZamowieniaService, ZamowieniaService>();
builder.Services.AddScoped<IStanowiskaService, StanowiskaService>();
builder.Services.AddScoped<IOpinieService, OpinieService>();
builder.Services.AddScoped<IRaportyService, RaportyService>();
builder.Services.AddScoped<IRaportPdfService, RaportPdfService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUstawieniaKontaktoweService, UstawieniaKontaktoweService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed ról
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedRolesAsync(services);
}

app.Run();