using System;
using System.Collections.Generic;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClubData.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ActiveClubData.Models;
using Microsoft.AspNetCore.Identity;

public partial class ActiveClubContext : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    int,
    IdentityUserClaim<int>,
    IdentityUserRole<int>,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>
{
    public ActiveClubContext()
    {
    }

    public ActiveClubContext(DbContextOptions<ActiveClubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aktualnosci> Aktualnoscis { get; set; }
    public virtual DbSet<CzlonkostwaUzytkownika> CzlonkostwaUzytkownikas { get; set; }
    public virtual DbSet<UstawieniaKontaktowe> UstawieniaKontaktowes { get; set; }

    public virtual DbSet<WejscieKlubowe> WejsciaKlubowes { get; set; }
    public virtual DbSet<DostepnoscTrenera> DostepnoscTreneras { get; set; }
    public virtual DbSet<GodzinyOtwarcium> GodzinyOtwarcia { get; set; }
    public virtual DbSet<HarmonogramZajec> HarmonogramZajecs { get; set; }
    public virtual DbSet<Kluby> Klubies { get; set; }
    public virtual DbSet<MetodyPlatnosci> MetodyPlatnoscis { get; set; }
    public virtual DbSet<OfertaKlubuCzlonkostwo> OfertaKlubuCzlonkostwos { get; set; }
    public virtual DbSet<Opinie> Opinies { get; set; }
    public virtual DbSet<Platnosci> Platnoscis { get; set; }
    public virtual DbSet<PozycjeZamowienium> PozycjeZamowienia { get; set; }
    public virtual DbSet<PracownicyKlubu> PracownicyKlubus { get; set; }
    public virtual DbSet<Rabaty> Rabaties { get; set; }
    public virtual DbSet<RaportyDefinicje> RaportyDefinicjes { get; set; }
    public virtual DbSet<RaportyWygenerowane> RaportyWygenerowanes { get; set; }
    public virtual DbSet<RejestracjeNaZajecium> RejestracjeNaZajecia { get; set; }
    public virtual DbSet<RezerwacjeTrenera> RezerwacjeTreneras { get; set; }
    public virtual DbSet<RodzajeCzlonkostwa> RodzajeCzlonkostwas { get; set; }
    public virtual DbSet<RodzajeZajec> RodzajeZajecs { get; set; }
    public virtual DbSet<Sale> Sales { get; set; }
    public virtual DbSet<Stanowiska> Stanowiskas { get; set; }
    public virtual DbSet<TrenerKlub> TrenerKlubs { get; set; }
    public virtual DbSet<Trenerzy> Trenerzies { get; set; }
    public virtual DbSet<Zamowienium> Zamowienia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("AspNetUserRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("AspNetUserClaims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("AspNetUserLogins");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("AspNetUserTokens");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("AspNetRoleClaims");

        modelBuilder.Entity<Aktualnosci>(entity =>
        {
            entity.HasKey(e => e.AktualnoscId).HasName("PK_Aktualnosci_New");

            entity.ToTable("Aktualnosci");

            entity.Property(e => e.DataDodania)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");

            entity.Property(e => e.Tytul)
                .HasMaxLength(200);
        });

        modelBuilder.Entity<CzlonkostwaUzytkownika>(entity =>
        {
            entity.HasKey(e => e.CzlonkostwoId).HasName("PK_CzlonkostwaUzytkownika_New");

            entity.ToTable("CzlonkostwaUzytkownika");

            entity.Property(e => e.DataUtworzenia)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");

            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("AKTYWNE");

            entity.Property(e => e.CenaZakupu)
                .HasPrecision(10, 2);

            entity.HasOne(d => d.Klub).WithMany(p => p.CzlonkostwaUzytkownikas)
                .HasForeignKey(d => d.KlubId)
                .HasConstraintName("FK_CUNew_K");

            entity.HasOne(d => d.RodzajCzlonkostwa).WithMany(p => p.CzlonkostwaUzytkownikas)
                .HasForeignKey(d => d.RodzajCzlonkostwaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CUNew_R");

            entity.HasOne(d => d.Uzytkownik).WithMany()
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CUNew_U");
        });

        modelBuilder.Entity<DostepnoscTrenera>(entity =>
        {
            entity.HasKey(e => e.DostepnoscId).HasName("PK_DostepnoscTrenera_New");

            entity.ToTable("DostepnoscTrenera");

            entity.HasIndex(e => new { e.TrenerId, e.StartAt }, "IX_DTNew_T_S");

            entity.Property(e => e.EndAt).HasPrecision(0);
            entity.Property(e => e.StartAt).HasPrecision(0);

            entity.HasOne(d => d.Klub).WithMany(p => p.DostepnoscTreneras)
                .HasForeignKey(d => d.KlubId)
                .HasConstraintName("FK_DTNew_K");

            entity.HasOne(d => d.Trener).WithMany(p => p.DostepnoscTreneras)
                .HasForeignKey(d => d.TrenerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DTNew_T");
        });

        modelBuilder.Entity<GodzinyOtwarcium>(entity =>
        {
            entity.HasKey(e => e.GodzinyOtwarciaId).HasName("PK_GodzinyOtwarcia_New");

            entity.HasIndex(e => new { e.KlubId, e.DzienTygodnia }, "UX_GodzinyOtwarciaNew_KD").IsUnique();

            entity.Property(e => e.GodzinaDo).HasPrecision(0);
            entity.Property(e => e.GodzinaOd).HasPrecision(0);

            entity.HasOne(d => d.Klub).WithMany(p => p.GodzinyOtwarcia)
                .HasForeignKey(d => d.KlubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GodzinyOtwarciaNew_K");
        });

        modelBuilder.Entity<HarmonogramZajec>(entity =>
        {
            entity.HasKey(e => e.HarmonogramId).HasName("PK_HarmonogramZajec_New");

            entity.ToTable("HarmonogramZajec");

            entity.HasIndex(e => new { e.KlubId, e.StartAt }, "IX_HZNew_K_S");

            entity.Property(e => e.EndAt).HasPrecision(0);
            entity.Property(e => e.StartAt).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("AKTYWNE");

            entity.HasOne(d => d.Klub).WithMany(p => p.HarmonogramZajecs)
                .HasForeignKey(d => d.KlubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HZNew_K");

            entity.HasOne(d => d.RodzajZajec).WithMany(p => p.HarmonogramZajecs)
                .HasForeignKey(d => d.RodzajZajecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HZNew_R");

            entity.HasOne(d => d.Sala).WithMany(p => p.HarmonogramZajecs)
                .HasForeignKey(d => d.SalaId)
                .HasConstraintName("FK_HZNew_S");

            entity.HasOne(d => d.Trener).WithMany(p => p.HarmonogramZajecs)
                .HasForeignKey(d => d.TrenerId)
                .HasConstraintName("FK_HZNew_T");
        });

        modelBuilder.Entity<Kluby>(entity =>
        {
            entity.HasKey(e => e.KlubId).HasName("PK_Kluby_New");

            entity.ToTable("Kluby");

            entity.Property(e => e.CzyAktywny).HasDefaultValue(true);
            entity.Property(e => e.DataUtworzenia)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.EmailKontakt).HasMaxLength(320);
            entity.Property(e => e.KodPocztowy).HasMaxLength(20);
            entity.Property(e => e.Miasto).HasMaxLength(120);
            entity.Property(e => e.Nazwa).HasMaxLength(200);
            entity.Property(e => e.Telefon).HasMaxLength(30);
            entity.Property(e => e.Ulica).HasMaxLength(200);

           
        });

      

        modelBuilder.Entity<MetodyPlatnosci>(entity =>
        {
            entity.HasKey(e => e.MetodaPlatnosciId).HasName("PK_MetodyPlatnosci_New");

            entity.ToTable("MetodyPlatnosci");

            entity.HasIndex(e => e.Nazwa, "UX_MPNew_N").IsUnique();

            entity.Property(e => e.CzyAktywna).HasDefaultValue(true);
            entity.Property(e => e.Nazwa).HasMaxLength(120);
        });

        modelBuilder.Entity<OfertaKlubuCzlonkostwo>(entity =>
        {
            entity.HasKey(e => e.OfertaKlubuCzlonkostwoId);
            entity.ToTable("OfertaKlubuCzlonkostwo");

            entity.Property(e => e.Cena).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Opinie>(entity =>
        {
            entity.HasKey(e => e.OpiniaId).HasName("PK_Opinie_New");

            entity.ToTable("Opinie");

            entity.Property(e => e.DataDodania)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Komentarz).HasMaxLength(1000);

            entity.HasOne(d => d.Klub).WithMany(p => p.Opinies)
                .HasForeignKey(d => d.KlubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OpinieNew_K");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.Opinies)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OpinieNew_U");
        });

        modelBuilder.Entity<PozycjeZamowienium>(entity =>
        {
            entity.HasKey(e => e.PozycjaId).HasName("PK_PozycjeZamowienia_New");

            entity.Property(e => e.CenaJedn).HasPrecision(10, 2);
            entity.Property(e => e.KwotaBrutto).HasPrecision(10, 2);
            entity.Property(e => e.Ilosc).HasDefaultValue(1);
            entity.Property(e => e.Nazwa).HasMaxLength(200);
            entity.Property(e => e.Typ).HasMaxLength(40);

            entity.HasOne(d => d.Zamowienie).WithMany(p => p.PozycjeZamowienia)
                .HasForeignKey(d => d.ZamowienieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PZNew_Z");
        });
        modelBuilder.Entity<PracownicyKlubu>(entity =>
        {
            entity.HasKey(e => e.PracownikKlubId);

            entity.ToTable("PracownicyKlubu");

            entity.Property(e => e.CzyAktywny).HasDefaultValue(true);
            entity.Property(e => e.Uwagi).HasMaxLength(255);

            entity.HasOne(d => d.Klub)
                .WithMany(p => p.PracownicyKlubus)
                .HasForeignKey(d => d.KlubId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Stanowisko)
                .WithMany(p => p.PracownicyKlubus)
                .HasForeignKey(d => d.StanowiskoId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Uzytkownik)
                .WithMany()
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Platnosci>(entity =>
        {
            entity.HasKey(e => e.PlatnoscId);

            entity.ToTable("Platnosci");

            entity.Property(e => e.DataUtworzenia).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Kwota).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Provider).HasMaxLength(80);
            entity.Property(e => e.ProviderTransactionId).HasMaxLength(120);
            entity.Property(e => e.Status).HasMaxLength(30);
            entity.Property(e => e.Uwagi).HasMaxLength(255);
            entity.Property(e => e.Waluta)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.MetodaPlatnosci)
                .WithMany(p => p.Platnoscis)
                .HasForeignKey(d => d.MetodaPlatnosciId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Zamowienie)
                .WithMany(p => p.Platnoscis)
                .HasForeignKey(d => d.ZamowienieId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Rabaty>(entity =>
        {
            entity.HasKey(e => e.RabatId).HasName("PK_Rabaty_New");

            entity.ToTable("Rabaty");

            entity.HasIndex(e => e.Kod).IsUnique();

            entity.Property(e => e.CzyAktywny).HasDefaultValue(true);
            entity.Property(e => e.Kod).HasMaxLength(80);
            entity.Property(e => e.Typ).HasMaxLength(20);

            entity.Property(e => e.Wartosc).HasPrecision(10, 2);
            entity.Property(e => e.MinimalnaKwotaZamowienia).HasPrecision(10, 2);
        });

        modelBuilder.Entity<RaportyDefinicje>(entity =>
        {
            entity.HasKey(e => e.RaportDefinicjaId).HasName("PK_RaportyDefinicje_New");

            entity.ToTable("RaportyDefinicje");

            entity.HasIndex(e => e.Nazwa, "UX_RDNew_N").IsUnique();

            entity.Property(e => e.CzyAktywny).HasDefaultValue(true);
            entity.Property(e => e.Nazwa).HasMaxLength(160);
            entity.Property(e => e.Opis).HasMaxLength(500);
        });

        modelBuilder.Entity<RaportyWygenerowane>(entity =>
        {
            entity.HasKey(e => e.RaportWygenerowanyId).HasName("PK_RaportyWygenerowane_New");

            entity.ToTable("RaportyWygenerowane");

            entity.Property(e => e.DataGeneracji)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Format).HasMaxLength(10);
            entity.Property(e => e.PlikUrl).HasMaxLength(1000);

            entity.HasOne(d => d.WygenerowalUzytkownik)
      .WithMany(p => p.RaportyWygenerowanes)  // 
      .HasForeignKey(d => d.WygenerowalUzytkownikId)
      .HasConstraintName("FK_RWNew_U");

            entity.HasOne(d => d.WygenerowalUzytkownik).WithMany(p => p.RaportyWygenerowanes)
                .HasForeignKey(d => d.WygenerowalUzytkownikId)
                .HasConstraintName("FK_RWNew_U");
        });

        modelBuilder.Entity<RejestracjeNaZajecium>(entity =>
        {
            entity.HasKey(e => e.RejestracjaId).HasName("PK_RejestracjeNaZajecia_New");

            entity.HasIndex(e => new { e.HarmonogramId, e.UzytkownikId }, "UX_RNZNew_Uniq").IsUnique();

            entity.Property(e => e.DataRejestracji)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("ZAPISANY");

            entity.HasOne(d => d.Harmonogram).WithMany(p => p.RejestracjeNaZajecia)
                .HasForeignKey(d => d.HarmonogramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RNZNew_H");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.RejestracjeNaZajecia)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RNZNew_U");
        });

        modelBuilder.Entity<RezerwacjeTrenera>(entity =>
        {
            entity.HasKey(e => e.RezerwacjaId).HasName("PK_RezerwacjeTrenera_New");

            entity.ToTable("RezerwacjeTrenera");

            entity.HasIndex(e => new { e.TrenerId, e.StartAt }, "IX_RTNew_T_S");
            entity.HasIndex(e => new { e.UzytkownikId, e.StartAt }, "IX_RTNew_U_S");

            entity.Property(e => e.EndAt).HasPrecision(0);
            entity.Property(e => e.StartAt).HasPrecision(0);
            entity.Property(e => e.Cena).HasPrecision(10, 2);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("ZAREZERWOWANA");

            entity.HasOne(d => d.Klub).WithMany(p => p.RezerwacjeTreneras)
                .HasForeignKey(d => d.KlubId)
                .HasConstraintName("FK_RTNew_K");

            entity.HasOne(d => d.Trener).WithMany(p => p.RezerwacjeTreneras)
                .HasForeignKey(d => d.TrenerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RTNew_T");

            entity.HasOne(d => d.Uzytkownik).WithMany()
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RTNew_U");
        });

        modelBuilder.Entity<RodzajeCzlonkostwa>(entity =>
        {
            entity.HasKey(e => e.RodzajCzlonkostwaId).HasName("PK_RodzajeCzlonkostwa_New");

            entity.ToTable("RodzajeCzlonkostwa");

            entity.HasIndex(e => e.Nazwa, "UX_RCCNew_N").IsUnique();

            entity.Property(e => e.Cena).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CzyAktywny).HasDefaultValue(true);
            entity.Property(e => e.Nazwa).HasMaxLength(160);
            entity.Property(e => e.Opis).HasMaxLength(500);
        });

        modelBuilder.Entity<RodzajeZajec>(entity =>
        {
            entity.HasKey(e => e.RodzajZajecId).HasName("PK_RodzajeZajec_New");

            entity.ToTable("RodzajeZajec");

            entity.HasIndex(e => e.Nazwa, "UX_RodzajeZajecNew_N").IsUnique();

            entity.Property(e => e.Nazwa).HasMaxLength(160);
            entity.Property(e => e.Opis).HasMaxLength(500);
        });

        

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SalaId).HasName("PK_Sale_New");

            entity.ToTable("Sale");

            entity.HasIndex(e => new { e.KlubId, e.Nazwa }, "UX_SaleNew_KN").IsUnique();

            entity.Property(e => e.Nazwa).HasMaxLength(120);

            entity.HasOne(d => d.Klub).WithMany(p => p.Sales)
                .HasForeignKey(d => d.KlubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleNew_K");
        });

        modelBuilder.Entity<Stanowiska>(entity =>
        {
            entity.HasKey(e => e.StanowiskoId);

            entity.ToTable("Stanowiska");

            entity.HasIndex(e => e.Nazwa).IsUnique();

            entity.Property(e => e.CzyAktywne).HasDefaultValue(true);
            entity.Property(e => e.Nazwa).HasMaxLength(80);
            entity.Property(e => e.Opis).HasMaxLength(255);
        });

        modelBuilder.Entity<Trenerzy>(entity =>
        {
            entity.HasKey(e => e.TrenerId).HasName("PK_Trenerzy_New");

            entity.ToTable("Trenerzy");

            entity.Property(e => e.Imie).HasMaxLength(100);
            entity.Property(e => e.Nazwisko).HasMaxLength(100);
            entity.Property(e => e.StawkaZaGodzine).HasPrecision(10, 2);

            entity.HasOne(d => d.Uzytkownik).WithMany()
                .HasForeignKey(d => d.UzytkownikId)
                .HasConstraintName("FK_TrenerzyNew_U");
        });

        modelBuilder.Entity<TrenerKlub>(entity =>
        {
            entity.HasKey(e => new { e.TrenerId, e.KlubId });

            entity.ToTable("TrenerKlub");

            entity.HasOne(d => d.Klub)
                .WithMany(p => p.TrenerKlubs)
                .HasForeignKey(d => d.KlubId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Trener)
                .WithMany(p => p.TrenerKlubs)
                .HasForeignKey(d => d.TrenerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });







        modelBuilder.Entity<Zamowienium>(entity =>
        {
            entity.HasKey(e => e.ZamowienieId).HasName("PK_Zamowienia_New");

            entity.Property(e => e.DataUtworzenia)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("NOWE");
            entity.Property(e => e.SumaBrutto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Waluta)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("PLN")
                .IsFixedLength();

            entity.HasOne(d => d.Klub).WithMany(p => p.Zamowienia)
                .HasForeignKey(d => d.KlubId)
                .HasConstraintName("FK_ZNew_K");

            entity.HasOne(d => d.MetodaPlatnosci).WithMany(p => p.Zamowienia)
                .HasForeignKey(d => d.MetodaPlatnosciId)
                .HasConstraintName("FK_ZNew_MP");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.Zamowienia)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ZNew_U");
            entity.HasKey(e => e.ZamowienieId);

            entity.Property(e => e.SumaBrutto).HasPrecision(10, 2);
            entity.Property(e => e.KwotaRabatu).HasPrecision(10, 2);
        });

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
