using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActiveClubData.Migrations
{
    /// <inheritdoc />
    public partial class ForceAddKontaktAndWejscia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LiczbaWejscLimit",
                table: "CzlonkostwaUzytkownika",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LiczbaWejscWykorzystanych",
                table: "CzlonkostwaUzytkownika",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UstawieniaKontaktowes",
                columns: table => new
                {
                    UstawieniaKontaktoweId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GodzinyKontaktu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataModyfikacji = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UstawieniaKontaktowes", x => x.UstawieniaKontaktoweId);
                });

            migrationBuilder.CreateTable(
                name: "WejsciaKlubowes",
                columns: table => new
                {
                    WejscieKluboweId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzlonkostwoId = table.Column<int>(type: "int", nullable: false),
                    DataWejscia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KlubId = table.Column<int>(type: "int", nullable: true),
                    ZarejestrowalUzytkownikId = table.Column<int>(type: "int", nullable: true),
                    Uwagi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WejsciaKlubowes", x => x.WejscieKluboweId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UstawieniaKontaktowes");

            migrationBuilder.DropTable(
                name: "WejsciaKlubowes");

            migrationBuilder.DropColumn(
                name: "LiczbaWejscLimit",
                table: "CzlonkostwaUzytkownika");

            migrationBuilder.DropColumn(
                name: "LiczbaWejscWykorzystanych",
                table: "CzlonkostwaUzytkownika");
        }
    }
}
