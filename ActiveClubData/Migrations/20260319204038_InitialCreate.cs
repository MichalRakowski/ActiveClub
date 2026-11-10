using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActiveClubData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CzyAktywna = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    KlubDomyslnyId = table.Column<int>(type: "int", nullable: true),
                    Uwagi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kluby",
                columns: table => new
                {
                    KlubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Miasto = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Ulica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    KodPocztowy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EmailKontakt = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysdatetime())"),
                    DataModyfikacji = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kluby_New", x => x.KlubId);
                });

            migrationBuilder.CreateTable(
                name: "MetodyPlatnosci",
                columns: table => new
                {
                    MetodaPlatnosciId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CzyAktywna = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodyPlatnosci_New", x => x.MetodaPlatnosciId);
                });

            migrationBuilder.CreateTable(
                name: "Rabaty",
                columns: table => new
                {
                    RabatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Typ = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Wartosc = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DataOd = table.Column<DateOnly>(type: "date", nullable: true),
                    DataDo = table.Column<DateOnly>(type: "date", nullable: true),
                    MinimalnaKwotaZamowienia = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    MaksLiczbaUzyc = table.Column<int>(type: "int", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rabaty_New", x => x.RabatId);
                });

            migrationBuilder.CreateTable(
                name: "RaportyDefinicje",
                columns: table => new
                {
                    RaportDefinicjaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodRaportu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParametryJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaportyDefinicje_New", x => x.RaportDefinicjaId);
                });

            migrationBuilder.CreateTable(
                name: "RodzajeCzlonkostwa",
                columns: table => new
                {
                    RodzajCzlonkostwaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Cena = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TypRozliczenia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OkresDni = table.Column<int>(type: "int", nullable: true),
                    LiczbaWejsc = table.Column<int>(type: "int", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CzyWidoczneOnline = table.Column<bool>(type: "bit", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RodzajeCzlonkostwa_New", x => x.RodzajCzlonkostwaId);
                });

            migrationBuilder.CreateTable(
                name: "RodzajeZajec",
                columns: table => new
                {
                    RodzajZajecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CzasTrwaniaMin = table.Column<int>(type: "int", nullable: true),
                    CzyAktywne = table.Column<bool>(type: "bit", nullable: false),
                    CzyWidoczneOnline = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RodzajeZajec_New", x => x.RodzajZajecId);
                });

            migrationBuilder.CreateTable(
                name: "Stanowiska",
                columns: table => new
                {
                    StanowiskoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CzyAktywne = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stanowiska", x => x.StanowiskoId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trenerzy",
                columns: table => new
                {
                    TrenerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikId = table.Column<int>(type: "int", nullable: true),
                    Imie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecjalizacjaOpis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StawkaZaGodzine = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trenerzy_New", x => x.TrenerId);
                    table.ForeignKey(
                        name: "FK_TrenerzyNew_U",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trenerzy_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Aktualnosci",
                columns: table => new
                {
                    AktualnoscId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tytul = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KlubId = table.Column<int>(type: "int", nullable: true),
                    CzyWidoczna = table.Column<bool>(type: "bit", nullable: true),
                    DataPublikacji = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDodania = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, defaultValueSql: "(sysdatetime())"),
                    DodanePrzezUzytkownikId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktualnosci_New", x => x.AktualnoscId);
                    table.ForeignKey(
                        name: "FK_Aktualnosci_AspNetUsers_DodanePrzezUzytkownikId",
                        column: x => x.DodanePrzezUzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Aktualnosci_Kluby_KlubId",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                });

            migrationBuilder.CreateTable(
                name: "GodzinyOtwarcia",
                columns: table => new
                {
                    GodzinyOtwarciaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlubId = table.Column<int>(type: "int", nullable: false),
                    DzienTygodnia = table.Column<byte>(type: "tinyint", nullable: false),
                    GodzinaOd = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    GodzinaDo = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    CzyZamkniete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GodzinyOtwarcia_New", x => x.GodzinyOtwarciaId);
                    table.ForeignKey(
                        name: "FK_GodzinyOtwarciaNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                });

            migrationBuilder.CreateTable(
                name: "Opinie",
                columns: table => new
                {
                    OpiniaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlubId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    Ocena = table.Column<byte>(type: "tinyint", nullable: false),
                    Komentarz = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CzyWidoczna = table.Column<bool>(type: "bit", nullable: false),
                    DataDodania = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opinie_New", x => x.OpiniaId);
                    table.ForeignKey(
                        name: "FK_OpinieNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_OpinieNew_U",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    SalaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlubId = table.Column<int>(type: "int", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Pojemnosc = table.Column<int>(type: "int", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CzyAktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale_New", x => x.SalaId);
                    table.ForeignKey(
                        name: "FK_SaleNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                });

            migrationBuilder.CreateTable(
                name: "Zamowienia",
                columns: table => new
                {
                    ZamowienieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumerZamowienia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    KlubId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "NOWE"),
                    SumaBrutto = table.Column<decimal>(type: "decimal(18,2)", precision: 10, scale: 2, nullable: false),
                    KwotaRabatu = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Waluta = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false, defaultValue: "PLN"),
                    MetodaPlatnosciId = table.Column<int>(type: "int", nullable: true),
                    RabatId = table.Column<int>(type: "int", nullable: true),
                    TypKanalu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysdatetime())"),
                    DataOplacenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UtworzylUzytkownikId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zamowienia_New", x => x.ZamowienieId);
                    table.ForeignKey(
                        name: "FK_ZNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_ZNew_MP",
                        column: x => x.MetodaPlatnosciId,
                        principalTable: "MetodyPlatnosci",
                        principalColumn: "MetodaPlatnosciId");
                    table.ForeignKey(
                        name: "FK_ZNew_U",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Zamowienia_Rabaty_RabatId",
                        column: x => x.RabatId,
                        principalTable: "Rabaty",
                        principalColumn: "RabatId");
                });

            migrationBuilder.CreateTable(
                name: "RaportyWygenerowane",
                columns: table => new
                {
                    RaportWygenerowanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaportDefinicjaId = table.Column<int>(type: "int", nullable: false),
                    WygenerowalUzytkownikId = table.Column<int>(type: "int", nullable: true),
                    Format = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlikUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ParametryJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataGeneracji = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaportyWygenerowane_New", x => x.RaportWygenerowanyId);
                    table.ForeignKey(
                        name: "FK_RWNew_U",
                        column: x => x.WygenerowalUzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RaportyWygenerowane_RaportyDefinicje_RaportDefinicjaId",
                        column: x => x.RaportDefinicjaId,
                        principalTable: "RaportyDefinicje",
                        principalColumn: "RaportDefinicjaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfertaKlubuCzlonkostwo",
                columns: table => new
                {
                    OfertaKlubuCzlonkostwoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlubId = table.Column<int>(type: "int", nullable: false),
                    RodzajCzlonkostwaId = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Waluta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CzyAktywna = table.Column<bool>(type: "bit", nullable: false),
                    CzyWidoczneOnline = table.Column<bool>(type: "bit", nullable: false),
                    DataOd = table.Column<DateOnly>(type: "date", nullable: true),
                    DataDo = table.Column<DateOnly>(type: "date", nullable: true),
                    Kolejnosc = table.Column<int>(type: "int", nullable: true),
                    OpisMarketingowy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfertaKlubuCzlonkostwo", x => x.OfertaKlubuCzlonkostwoId);
                    table.ForeignKey(
                        name: "FK_OfertaKlubuCzlonkostwo_Kluby_KlubId",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfertaKlubuCzlonkostwo_RodzajeCzlonkostwa_RodzajCzlonkostwaId",
                        column: x => x.RodzajCzlonkostwaId,
                        principalTable: "RodzajeCzlonkostwa",
                        principalColumn: "RodzajCzlonkostwaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PracownicyKlubu",
                columns: table => new
                {
                    PracownikKlubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    KlubId = table.Column<int>(type: "int", nullable: false),
                    StanowiskoId = table.Column<int>(type: "int", nullable: false),
                    DataOd = table.Column<DateOnly>(type: "date", nullable: true),
                    DataDo = table.Column<DateOnly>(type: "date", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Uwagi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracownicyKlubu", x => x.PracownikKlubId);
                    table.ForeignKey(
                        name: "FK_PracownicyKlubu_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PracownicyKlubu_AspNetUsers_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PracownicyKlubu_Kluby_KlubId",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_PracownicyKlubu_Stanowiska_StanowiskoId",
                        column: x => x.StanowiskoId,
                        principalTable: "Stanowiska",
                        principalColumn: "StanowiskoId");
                });

            migrationBuilder.CreateTable(
                name: "DostepnoscTrenera",
                columns: table => new
                {
                    DostepnoscId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    KlubId = table.Column<int>(type: "int", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtworzonoAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DostepnoscTrenera_New", x => x.DostepnoscId);
                    table.ForeignKey(
                        name: "FK_DTNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_DTNew_T",
                        column: x => x.TrenerId,
                        principalTable: "Trenerzy",
                        principalColumn: "TrenerId");
                });

            migrationBuilder.CreateTable(
                name: "RezerwacjeTrenera",
                columns: table => new
                {
                    RezerwacjaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    KlubId = table.Column<int>(type: "int", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValue: "ZAREZERWOWANA"),
                    Cena = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezerwacjeTrenera_New", x => x.RezerwacjaId);
                    table.ForeignKey(
                        name: "FK_RTNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_RTNew_T",
                        column: x => x.TrenerId,
                        principalTable: "Trenerzy",
                        principalColumn: "TrenerId");
                    table.ForeignKey(
                        name: "FK_RTNew_U",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RezerwacjeTrenera_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrenerKlub",
                columns: table => new
                {
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    KlubId = table.Column<int>(type: "int", nullable: false),
                    DataOd = table.Column<DateOnly>(type: "date", nullable: true),
                    DataDo = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrenerKlub", x => new { x.TrenerId, x.KlubId });
                    table.ForeignKey(
                        name: "FK_TrenerKlub_Kluby_KlubId",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_TrenerKlub_Trenerzy_TrenerId",
                        column: x => x.TrenerId,
                        principalTable: "Trenerzy",
                        principalColumn: "TrenerId");
                });

            migrationBuilder.CreateTable(
                name: "HarmonogramZajec",
                columns: table => new
                {
                    HarmonogramId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlubId = table.Column<int>(type: "int", nullable: false),
                    RodzajZajecId = table.Column<int>(type: "int", nullable: false),
                    SalaId = table.Column<int>(type: "int", nullable: true),
                    TrenerId = table.Column<int>(type: "int", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    LimitMiejsc = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "AKTYWNE"),
                    CzyWidoczneOnline = table.Column<bool>(type: "bit", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtworzylUzytkownikId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarmonogramZajec_New", x => x.HarmonogramId);
                    table.ForeignKey(
                        name: "FK_HZNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_HZNew_R",
                        column: x => x.RodzajZajecId,
                        principalTable: "RodzajeZajec",
                        principalColumn: "RodzajZajecId");
                    table.ForeignKey(
                        name: "FK_HZNew_S",
                        column: x => x.SalaId,
                        principalTable: "Sale",
                        principalColumn: "SalaId");
                    table.ForeignKey(
                        name: "FK_HZNew_T",
                        column: x => x.TrenerId,
                        principalTable: "Trenerzy",
                        principalColumn: "TrenerId");
                });

            migrationBuilder.CreateTable(
                name: "Platnosci",
                columns: table => new
                {
                    PlatnoscId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZamowienieId = table.Column<int>(type: "int", nullable: false),
                    MetodaPlatnosciId = table.Column<int>(type: "int", nullable: false),
                    Kwota = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Waluta = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ProviderTransactionId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    DataPlatnosci = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    Uwagi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platnosci", x => x.PlatnoscId);
                    table.ForeignKey(
                        name: "FK_Platnosci_MetodyPlatnosci_MetodaPlatnosciId",
                        column: x => x.MetodaPlatnosciId,
                        principalTable: "MetodyPlatnosci",
                        principalColumn: "MetodaPlatnosciId");
                    table.ForeignKey(
                        name: "FK_Platnosci_Zamowienia_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienia",
                        principalColumn: "ZamowienieId");
                });

            migrationBuilder.CreateTable(
                name: "PozycjeZamowienia",
                columns: table => new
                {
                    PozycjaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZamowienieId = table.Column<int>(type: "int", nullable: false),
                    Typ = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    RefId = table.Column<int>(type: "int", nullable: true),
                    Nazwa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ilosc = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CenaJedn = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    KwotaBrutto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PozycjeZamowienia_New", x => x.PozycjaId);
                    table.ForeignKey(
                        name: "FK_PZNew_Z",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienia",
                        principalColumn: "ZamowienieId");
                });

            migrationBuilder.CreateTable(
                name: "CzlonkostwaUzytkownika",
                columns: table => new
                {
                    CzlonkostwoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    OfertaKlubuCzlonkostwoId = table.Column<int>(type: "int", nullable: true),
                    RodzajCzlonkostwaId = table.Column<int>(type: "int", nullable: true),
                    KlubId = table.Column<int>(type: "int", nullable: true),
                    StartAt = table.Column<DateOnly>(type: "date", nullable: true),
                    EndAt = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValue: "AKTYWNE"),
                    CenaZakupu = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    ZamowienieId = table.Column<int>(type: "int", nullable: true),
                    NadanePrzezUzytkownikId = table.Column<int>(type: "int", nullable: true),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, defaultValueSql: "(sysdatetime())"),
                    DataAktywacji = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataAnulowania = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PowodAnulowania = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CzlonkostwaUzytkownika_New", x => x.CzlonkostwoId);
                    table.ForeignKey(
                        name: "FK_CUNew_K",
                        column: x => x.KlubId,
                        principalTable: "Kluby",
                        principalColumn: "KlubId");
                    table.ForeignKey(
                        name: "FK_CUNew_R",
                        column: x => x.RodzajCzlonkostwaId,
                        principalTable: "RodzajeCzlonkostwa",
                        principalColumn: "RodzajCzlonkostwaId");
                    table.ForeignKey(
                        name: "FK_CUNew_U",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CzlonkostwaUzytkownika_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CzlonkostwaUzytkownika_OfertaKlubuCzlonkostwo_OfertaKlubuCzlonkostwoId",
                        column: x => x.OfertaKlubuCzlonkostwoId,
                        principalTable: "OfertaKlubuCzlonkostwo",
                        principalColumn: "OfertaKlubuCzlonkostwoId");
                    table.ForeignKey(
                        name: "FK_CzlonkostwaUzytkownika_Zamowienia_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienia",
                        principalColumn: "ZamowienieId");
                });

            migrationBuilder.CreateTable(
                name: "RejestracjeNaZajecia",
                columns: table => new
                {
                    RejestracjaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarmonogramId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValue: "ZAPISANY"),
                    DataRejestracji = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true, defaultValueSql: "(sysdatetime())"),
                    Zrodlo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejestracjeNaZajecia_New", x => x.RejestracjaId);
                    table.ForeignKey(
                        name: "FK_RNZNew_H",
                        column: x => x.HarmonogramId,
                        principalTable: "HarmonogramZajec",
                        principalColumn: "HarmonogramId");
                    table.ForeignKey(
                        name: "FK_RNZNew_U",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aktualnosci_DodanePrzezUzytkownikId",
                table: "Aktualnosci",
                column: "DodanePrzezUzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Aktualnosci_KlubId",
                table: "Aktualnosci",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CzlonkostwaUzytkownika_ApplicationUserId",
                table: "CzlonkostwaUzytkownika",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CzlonkostwaUzytkownika_KlubId",
                table: "CzlonkostwaUzytkownika",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_CzlonkostwaUzytkownika_OfertaKlubuCzlonkostwoId",
                table: "CzlonkostwaUzytkownika",
                column: "OfertaKlubuCzlonkostwoId");

            migrationBuilder.CreateIndex(
                name: "IX_CzlonkostwaUzytkownika_RodzajCzlonkostwaId",
                table: "CzlonkostwaUzytkownika",
                column: "RodzajCzlonkostwaId");

            migrationBuilder.CreateIndex(
                name: "IX_CzlonkostwaUzytkownika_UzytkownikId",
                table: "CzlonkostwaUzytkownika",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_CzlonkostwaUzytkownika_ZamowienieId",
                table: "CzlonkostwaUzytkownika",
                column: "ZamowienieId");

            migrationBuilder.CreateIndex(
                name: "IX_DostepnoscTrenera_KlubId",
                table: "DostepnoscTrenera",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_DTNew_T_S",
                table: "DostepnoscTrenera",
                columns: new[] { "TrenerId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "UX_GodzinyOtwarciaNew_KD",
                table: "GodzinyOtwarcia",
                columns: new[] { "KlubId", "DzienTygodnia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HarmonogramZajec_RodzajZajecId",
                table: "HarmonogramZajec",
                column: "RodzajZajecId");

            migrationBuilder.CreateIndex(
                name: "IX_HarmonogramZajec_SalaId",
                table: "HarmonogramZajec",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_HarmonogramZajec_TrenerId",
                table: "HarmonogramZajec",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_HZNew_K_S",
                table: "HarmonogramZajec",
                columns: new[] { "KlubId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "UX_MPNew_N",
                table: "MetodyPlatnosci",
                column: "Nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfertaKlubuCzlonkostwo_KlubId",
                table: "OfertaKlubuCzlonkostwo",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_OfertaKlubuCzlonkostwo_RodzajCzlonkostwaId",
                table: "OfertaKlubuCzlonkostwo",
                column: "RodzajCzlonkostwaId");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_KlubId",
                table: "Opinie",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_UzytkownikId",
                table: "Opinie",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Platnosci_MetodaPlatnosciId",
                table: "Platnosci",
                column: "MetodaPlatnosciId");

            migrationBuilder.CreateIndex(
                name: "IX_Platnosci_ZamowienieId",
                table: "Platnosci",
                column: "ZamowienieId");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjeZamowienia_ZamowienieId",
                table: "PozycjeZamowienia",
                column: "ZamowienieId");

            migrationBuilder.CreateIndex(
                name: "IX_PracownicyKlubu_ApplicationUserId",
                table: "PracownicyKlubu",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PracownicyKlubu_KlubId",
                table: "PracownicyKlubu",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_PracownicyKlubu_StanowiskoId",
                table: "PracownicyKlubu",
                column: "StanowiskoId");

            migrationBuilder.CreateIndex(
                name: "IX_PracownicyKlubu_UzytkownikId",
                table: "PracownicyKlubu",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rabaty_Kod",
                table: "Rabaty",
                column: "Kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_RDNew_N",
                table: "RaportyDefinicje",
                column: "Nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaportyWygenerowane_RaportDefinicjaId",
                table: "RaportyWygenerowane",
                column: "RaportDefinicjaId");

            migrationBuilder.CreateIndex(
                name: "IX_RaportyWygenerowane_WygenerowalUzytkownikId",
                table: "RaportyWygenerowane",
                column: "WygenerowalUzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_RejestracjeNaZajecia_UzytkownikId",
                table: "RejestracjeNaZajecia",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "UX_RNZNew_Uniq",
                table: "RejestracjeNaZajecia",
                columns: new[] { "HarmonogramId", "UzytkownikId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RezerwacjeTrenera_ApplicationUserId",
                table: "RezerwacjeTrenera",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RezerwacjeTrenera_KlubId",
                table: "RezerwacjeTrenera",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_RTNew_T_S",
                table: "RezerwacjeTrenera",
                columns: new[] { "TrenerId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RTNew_U_S",
                table: "RezerwacjeTrenera",
                columns: new[] { "UzytkownikId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "UX_RCCNew_N",
                table: "RodzajeCzlonkostwa",
                column: "Nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_RodzajeZajecNew_N",
                table: "RodzajeZajec",
                column: "Nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_SaleNew_KN",
                table: "Sale",
                columns: new[] { "KlubId", "Nazwa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stanowiska_Nazwa",
                table: "Stanowiska",
                column: "Nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrenerKlub_KlubId",
                table: "TrenerKlub",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_Trenerzy_ApplicationUserId",
                table: "Trenerzy",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trenerzy_UzytkownikId",
                table: "Trenerzy",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_KlubId",
                table: "Zamowienia",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_MetodaPlatnosciId",
                table: "Zamowienia",
                column: "MetodaPlatnosciId");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_RabatId",
                table: "Zamowienia",
                column: "RabatId");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_UzytkownikId",
                table: "Zamowienia",
                column: "UzytkownikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aktualnosci");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CzlonkostwaUzytkownika");

            migrationBuilder.DropTable(
                name: "DostepnoscTrenera");

            migrationBuilder.DropTable(
                name: "GodzinyOtwarcia");

            migrationBuilder.DropTable(
                name: "Opinie");

            migrationBuilder.DropTable(
                name: "Platnosci");

            migrationBuilder.DropTable(
                name: "PozycjeZamowienia");

            migrationBuilder.DropTable(
                name: "PracownicyKlubu");

            migrationBuilder.DropTable(
                name: "RaportyWygenerowane");

            migrationBuilder.DropTable(
                name: "RejestracjeNaZajecia");

            migrationBuilder.DropTable(
                name: "RezerwacjeTrenera");

            migrationBuilder.DropTable(
                name: "TrenerKlub");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OfertaKlubuCzlonkostwo");

            migrationBuilder.DropTable(
                name: "Zamowienia");

            migrationBuilder.DropTable(
                name: "Stanowiska");

            migrationBuilder.DropTable(
                name: "RaportyDefinicje");

            migrationBuilder.DropTable(
                name: "HarmonogramZajec");

            migrationBuilder.DropTable(
                name: "RodzajeCzlonkostwa");

            migrationBuilder.DropTable(
                name: "MetodyPlatnosci");

            migrationBuilder.DropTable(
                name: "Rabaty");

            migrationBuilder.DropTable(
                name: "RodzajeZajec");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "Trenerzy");

            migrationBuilder.DropTable(
                name: "Kluby");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
