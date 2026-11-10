using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActiveClub.PortalWWW.Migrations
{
    /// <inheritdoc />
    public partial class AddKontaktAndWejscia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aktualnosci",
                columns: table => new
                {
                    AktualnoscID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tytul = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Aktualno__3B171DCD01948B3D", x => x.AktualnoscID);
                });

            migrationBuilder.CreateTable(
                name: "Kluby",
                columns: table => new
                {
                    KlubID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kluby__A0FD579B8EB511A9", x => x.KlubID);
                });

            migrationBuilder.CreateTable(
                name: "LogiSystemowe",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Data = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LogiSyst__5E5499A85121A8F0", x => x.LogID);
                });

            migrationBuilder.CreateTable(
                name: "MetodyPlatnosci",
                columns: table => new
                {
                    MetodaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MetodyPl__48FC0EACCD91CDEA", x => x.MetodaID);
                });

            migrationBuilder.CreateTable(
                name: "PlanyTreningowe",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlanyTre__755C22D781C490C0", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "Rabaty",
                columns: table => new
                {
                    RabatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Procent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rabaty__0D0A228B44840ECB", x => x.RabatID);
                });

            migrationBuilder.CreateTable(
                name: "Raporty",
                columns: table => new
                {
                    RaportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Raporty__234C4E1B1FEC8BC8", x => x.RaportID);
                });

            migrationBuilder.CreateTable(
                name: "RodzajeCzlonkostwa",
                columns: table => new
                {
                    RodzajID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cena = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CzasTrwaniaDni = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RodzajeC__19A2F39D81CC41A1", x => x.RodzajID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RolaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__8DF280FBED050F7E", x => x.RolaID);
                });

            migrationBuilder.CreateTable(
                name: "Sprzet",
                columns: table => new
                {
                    SprzetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sprzet__B7888C95AD624CDA", x => x.SprzetID);
                });

            migrationBuilder.CreateTable(
                name: "SzablonyRaportow",
                columns: table => new
                {
                    SzablonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Szablony__40D35DF104006F35", x => x.SzablonID);
                });

            migrationBuilder.CreateTable(
                name: "Trenerzy",
                columns: table => new
                {
                    TrenerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Trenerzy__ABE1A132345487B5", x => x.TrenerID);
                });

            migrationBuilder.CreateTable(
                name: "Uprawnienia",
                columns: table => new
                {
                    UprawnienieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Uprawnie__4C263D771EF25350", x => x.UprawnienieID);
                });

            migrationBuilder.CreateTable(
                name: "UstawieniaSystemu",
                columns: table => new
                {
                    UstawienieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Klucz = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Wartosc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ustawien__230D536F45891CC0", x => x.UstawienieID);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    UzytkownikID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Uzytkown__3E7072736CFA3771", x => x.UzytkownikID);
                });

            migrationBuilder.CreateTable(
                name: "Wyzwania",
                columns: table => new
                {
                    WyzwanieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wyzwania__BCA21DBDA9D84EF7", x => x.WyzwanieID);
                });

            migrationBuilder.CreateTable(
                name: "Zajecia",
                columns: table => new
                {
                    ZajeciaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Zajecia__D20A24FB71FE33BA", x => x.ZajeciaID);
                });

            migrationBuilder.CreateTable(
                name: "StatusSprzetu",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SprzetID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StatusSp__C8EE2043CA3CE492", x => x.StatusID);
                    table.ForeignKey(
                        name: "FK__StatusSpr__Sprze__7A672E12",
                        column: x => x.SprzetID,
                        principalTable: "Sprzet",
                        principalColumn: "SprzetID");
                });

            migrationBuilder.CreateTable(
                name: "HarmonogramyTrenerow",
                columns: table => new
                {
                    HarmonogramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrenerID = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Harmonog__E14DCB0E9D336774", x => x.HarmonogramID);
                    table.ForeignKey(
                        name: "FK__Harmonogr__Trene__6FE99F9F",
                        column: x => x.TrenerID,
                        principalTable: "Trenerzy",
                        principalColumn: "TrenerID");
                });

            migrationBuilder.CreateTable(
                name: "KlubyTrenerow",
                columns: table => new
                {
                    KlubID = table.Column<int>(type: "int", nullable: false),
                    TrenerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KlubyTre__9A434D88EE65F9BA", x => new { x.KlubID, x.TrenerID });
                    table.ForeignKey(
                        name: "FK__KlubyTren__KlubI__74AE54BC",
                        column: x => x.KlubID,
                        principalTable: "Kluby",
                        principalColumn: "KlubID");
                    table.ForeignKey(
                        name: "FK__KlubyTren__Trene__75A278F5",
                        column: x => x.TrenerID,
                        principalTable: "Trenerzy",
                        principalColumn: "TrenerID");
                });

            migrationBuilder.CreateTable(
                name: "UprawnieniaRol",
                columns: table => new
                {
                    RolaID = table.Column<int>(type: "int", nullable: false),
                    UprawnienieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Uprawnie__E930E32C1761F162", x => new { x.RolaID, x.UprawnienieID });
                    table.ForeignKey(
                        name: "FK__Uprawnien__RolaI__4222D4EF",
                        column: x => x.RolaID,
                        principalTable: "Role",
                        principalColumn: "RolaID");
                    table.ForeignKey(
                        name: "FK__Uprawnien__Upraw__4316F928",
                        column: x => x.UprawnienieID,
                        principalTable: "Uprawnienia",
                        principalColumn: "UprawnienieID");
                });

            migrationBuilder.CreateTable(
                name: "Czlonkostwa",
                columns: table => new
                {
                    CzlonkostwoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikID = table.Column<int>(type: "int", nullable: false),
                    RodzajID = table.Column<int>(type: "int", nullable: false),
                    DataRozpoczecia = table.Column<DateOnly>(type: "date", nullable: true),
                    DataZakonczenia = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Czlonkos__D6A7DDC7F5027EFC", x => x.CzlonkostwoID);
                    table.ForeignKey(
                        name: "FK__Czlonkost__Rodza__4F7CD00D",
                        column: x => x.RodzajID,
                        principalTable: "RodzajeCzlonkostwa",
                        principalColumn: "RodzajID");
                    table.ForeignKey(
                        name: "FK__Czlonkost__Uzytk__4E88ABD4",
                        column: x => x.UzytkownikID,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikID");
                });

            migrationBuilder.CreateTable(
                name: "Opinie",
                columns: table => new
                {
                    OpiniaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikID = table.Column<int>(type: "int", nullable: true),
                    Tresc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Opinie__6F9FEB93DDAA0D18", x => x.OpiniaID);
                    table.ForeignKey(
                        name: "FK__Opinie__Uzytkown__07C12930",
                        column: x => x.UzytkownikID,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikID");
                });

            migrationBuilder.CreateTable(
                name: "PostepyUzytkownika",
                columns: table => new
                {
                    PostepID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikID = table.Column<int>(type: "int", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Data = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PostepyU__B3CE89198F5CFFB5", x => x.PostepID);
                    table.ForeignKey(
                        name: "FK__PostepyUz__Uzytk__6B24EA82",
                        column: x => x.UzytkownikID,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikID");
                });

            migrationBuilder.CreateTable(
                name: "RoleUzytkownikow",
                columns: table => new
                {
                    UzytkownikID = table.Column<int>(type: "int", nullable: false),
                    RolaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RoleUzyt__96AF5A7CC1908372", x => new { x.UzytkownikID, x.RolaID });
                    table.ForeignKey(
                        name: "FK__RoleUzytk__RolaI__46E78A0C",
                        column: x => x.RolaID,
                        principalTable: "Role",
                        principalColumn: "RolaID");
                    table.ForeignKey(
                        name: "FK__RoleUzytk__Uzytk__45F365D3",
                        column: x => x.UzytkownikID,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikID");
                });

            migrationBuilder.CreateTable(
                name: "UstawieniaUzytkownika",
                columns: table => new
                {
                    UstawienieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikID = table.Column<int>(type: "int", nullable: false),
                    Klucz = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Wartosc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ustawien__230D536FAEE86BA8", x => x.UstawienieID);
                    table.ForeignKey(
                        name: "FK__Ustawieni__Uzytk__49C3F6B7",
                        column: x => x.UzytkownikID,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikID");
                });

            migrationBuilder.CreateTable(
                name: "HarmonogramZajec",
                columns: table => new
                {
                    HarmonogramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZajeciaID = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Harmonog__E14DCB0E546137E3", x => x.HarmonogramID);
                    table.ForeignKey(
                        name: "FK__Harmonogr__Zajec__60A75C0F",
                        column: x => x.ZajeciaID,
                        principalTable: "Zajecia",
                        principalColumn: "ZajeciaID");
                });

            migrationBuilder.CreateTable(
                name: "HistoriaCzlonkostwa",
                columns: table => new
                {
                    HistoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzlonkostwoID = table.Column<int>(type: "int", nullable: false),
                    DataZmiany = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Historia__0F6B1111F628A157", x => x.HistoriaID);
                    table.ForeignKey(
                        name: "FK__HistoriaC__Czlon__534D60F1",
                        column: x => x.CzlonkostwoID,
                        principalTable: "Czlonkostwa",
                        principalColumn: "CzlonkostwoID");
                });

            migrationBuilder.CreateTable(
                name: "Platnosci",
                columns: table => new
                {
                    PlatnoscID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzlonkostwoID = table.Column<int>(type: "int", nullable: false),
                    MetodaID = table.Column<int>(type: "int", nullable: false),
                    Kwota = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DataPlatnosci = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Platnosc__AB9E39AA249F8638", x => x.PlatnoscID);
                    table.ForeignKey(
                        name: "FK__Platnosci__Czlon__59063A47",
                        column: x => x.CzlonkostwoID,
                        principalTable: "Czlonkostwa",
                        principalColumn: "CzlonkostwoID");
                    table.ForeignKey(
                        name: "FK__Platnosci__Metod__59FA5E80",
                        column: x => x.MetodaID,
                        principalTable: "MetodyPlatnosci",
                        principalColumn: "MetodaID");
                });

            migrationBuilder.CreateTable(
                name: "RejestracjeNaZajecia",
                columns: table => new
                {
                    RejestracjaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarmonogramID = table.Column<int>(type: "int", nullable: true),
                    UzytkownikID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rejestra__91E27E7406304837", x => x.RejestracjaID);
                    table.ForeignKey(
                        name: "FK__Rejestrac__Harmo__6383C8BA",
                        column: x => x.HarmonogramID,
                        principalTable: "HarmonogramZajec",
                        principalColumn: "HarmonogramID");
                    table.ForeignKey(
                        name: "FK__Rejestrac__Uzytk__6477ECF3",
                        column: x => x.UzytkownikID,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Czlonkostwa_RodzajID",
                table: "Czlonkostwa",
                column: "RodzajID");

            migrationBuilder.CreateIndex(
                name: "IX_Czlonkostwa_UzytkownikID",
                table: "Czlonkostwa",
                column: "UzytkownikID");

            migrationBuilder.CreateIndex(
                name: "IX_HarmonogramyTrenerow_TrenerID",
                table: "HarmonogramyTrenerow",
                column: "TrenerID");

            migrationBuilder.CreateIndex(
                name: "IX_HarmonogramZajec_ZajeciaID",
                table: "HarmonogramZajec",
                column: "ZajeciaID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriaCzlonkostwa_CzlonkostwoID",
                table: "HistoriaCzlonkostwa",
                column: "CzlonkostwoID");

            migrationBuilder.CreateIndex(
                name: "IX_KlubyTrenerow_TrenerID",
                table: "KlubyTrenerow",
                column: "TrenerID");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_UzytkownikID",
                table: "Opinie",
                column: "UzytkownikID");

            migrationBuilder.CreateIndex(
                name: "IX_Platnosci_CzlonkostwoID",
                table: "Platnosci",
                column: "CzlonkostwoID");

            migrationBuilder.CreateIndex(
                name: "IX_Platnosci_MetodaID",
                table: "Platnosci",
                column: "MetodaID");

            migrationBuilder.CreateIndex(
                name: "IX_PostepyUzytkownika_UzytkownikID",
                table: "PostepyUzytkownika",
                column: "UzytkownikID");

            migrationBuilder.CreateIndex(
                name: "IX_RejestracjeNaZajecia_HarmonogramID",
                table: "RejestracjeNaZajecia",
                column: "HarmonogramID");

            migrationBuilder.CreateIndex(
                name: "IX_RejestracjeNaZajecia_UzytkownikID",
                table: "RejestracjeNaZajecia",
                column: "UzytkownikID");

            migrationBuilder.CreateIndex(
                name: "UQ__Role__602223FFC0EBFFA5",
                table: "Role",
                column: "Nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleUzytkownikow_RolaID",
                table: "RoleUzytkownikow",
                column: "RolaID");

            migrationBuilder.CreateIndex(
                name: "IX_StatusSprzetu_SprzetID",
                table: "StatusSprzetu",
                column: "SprzetID");

            migrationBuilder.CreateIndex(
                name: "UQ__Uprawnie__602223FFB3898074",
                table: "Uprawnienia",
                column: "Nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UprawnieniaRol_UprawnienieID",
                table: "UprawnieniaRol",
                column: "UprawnienieID");

            migrationBuilder.CreateIndex(
                name: "IX_UstawieniaUzytkownika_UzytkownikID",
                table: "UstawieniaUzytkownika",
                column: "UzytkownikID");

            migrationBuilder.CreateIndex(
                name: "UQ__Uzytkown__A9D105349E397662",
                table: "Uzytkownicy",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aktualnosci");

            migrationBuilder.DropTable(
                name: "HarmonogramyTrenerow");

            migrationBuilder.DropTable(
                name: "HistoriaCzlonkostwa");

            migrationBuilder.DropTable(
                name: "KlubyTrenerow");

            migrationBuilder.DropTable(
                name: "LogiSystemowe");

            migrationBuilder.DropTable(
                name: "Opinie");

            migrationBuilder.DropTable(
                name: "PlanyTreningowe");

            migrationBuilder.DropTable(
                name: "Platnosci");

            migrationBuilder.DropTable(
                name: "PostepyUzytkownika");

            migrationBuilder.DropTable(
                name: "Rabaty");

            migrationBuilder.DropTable(
                name: "Raporty");

            migrationBuilder.DropTable(
                name: "RejestracjeNaZajecia");

            migrationBuilder.DropTable(
                name: "RoleUzytkownikow");

            migrationBuilder.DropTable(
                name: "StatusSprzetu");

            migrationBuilder.DropTable(
                name: "SzablonyRaportow");

            migrationBuilder.DropTable(
                name: "UprawnieniaRol");

            migrationBuilder.DropTable(
                name: "UstawieniaSystemu");

            migrationBuilder.DropTable(
                name: "UstawieniaUzytkownika");

            migrationBuilder.DropTable(
                name: "Wyzwania");

            migrationBuilder.DropTable(
                name: "Kluby");

            migrationBuilder.DropTable(
                name: "Trenerzy");

            migrationBuilder.DropTable(
                name: "Czlonkostwa");

            migrationBuilder.DropTable(
                name: "MetodyPlatnosci");

            migrationBuilder.DropTable(
                name: "HarmonogramZajec");

            migrationBuilder.DropTable(
                name: "Sprzet");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Uprawnienia");

            migrationBuilder.DropTable(
                name: "RodzajeCzlonkostwa");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");

            migrationBuilder.DropTable(
                name: "Zajecia");
        }
    }
}
