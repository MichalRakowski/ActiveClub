using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.PortalWWW.Models;

public partial class ActiveClubContext : DbContext
{
    public ActiveClubContext()
    {
    }

    public ActiveClubContext(DbContextOptions<ActiveClubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aktualnosci> Aktualnoscis { get; set; }

    public virtual DbSet<Czlonkostwa> Czlonkostwas { get; set; }

    public virtual DbSet<HarmonogramZajec> HarmonogramZajecs { get; set; }

    public virtual DbSet<HarmonogramyTrenerow> HarmonogramyTrenerows { get; set; }

    public virtual DbSet<HistoriaCzlonkostwa> HistoriaCzlonkostwas { get; set; }

    public virtual DbSet<Kluby> Klubies { get; set; }

    public virtual DbSet<LogiSystemowe> LogiSystemowes { get; set; }

    public virtual DbSet<MetodyPlatnosci> MetodyPlatnoscis { get; set; }

    public virtual DbSet<Opinie> Opinies { get; set; }

    public virtual DbSet<PlanyTreningowe> PlanyTreningowes { get; set; }

    public virtual DbSet<Platnosci> Platnoscis { get; set; }

    public virtual DbSet<PostepyUzytkownika> PostepyUzytkownikas { get; set; }

    public virtual DbSet<Rabaty> Rabaties { get; set; }

    public virtual DbSet<Raporty> Raporties { get; set; }

    public virtual DbSet<RejestracjeNaZajecium> RejestracjeNaZajecia { get; set; }

    public virtual DbSet<RodzajeCzlonkostwa> RodzajeCzlonkostwas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sprzet> Sprzets { get; set; }

    public virtual DbSet<StatusSprzetu> StatusSprzetus { get; set; }

    public virtual DbSet<SzablonyRaportow> SzablonyRaportows { get; set; }

    public virtual DbSet<Trenerzy> Trenerzies { get; set; }

    public virtual DbSet<Uprawnienium> Uprawnienia { get; set; }

    public virtual DbSet<UstawieniaSystemu> UstawieniaSystemus { get; set; }

    public virtual DbSet<UstawieniaUzytkownika> UstawieniaUzytkownikas { get; set; }

    public virtual DbSet<Uzytkownicy> Uzytkownicies { get; set; }

    public virtual DbSet<Wyzwanium> Wyzwania { get; set; }

    public virtual DbSet<Zajecium> Zajecia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-MS0D67O\\SQLEXPRESS;Database=ActiveClub;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aktualnosci>(entity =>
        {
            entity.HasKey(e => e.AktualnoscId).HasName("PK__Aktualno__3B171DCD01948B3D");

            entity.ToTable("Aktualnosci");

            entity.Property(e => e.AktualnoscId).HasColumnName("AktualnoscID");
            entity.Property(e => e.Tytul).HasMaxLength(100);
        });

        modelBuilder.Entity<Czlonkostwa>(entity =>
        {
            entity.HasKey(e => e.CzlonkostwoId).HasName("PK__Czlonkos__D6A7DDC7F5027EFC");

            entity.ToTable("Czlonkostwa");

            entity.Property(e => e.CzlonkostwoId).HasColumnName("CzlonkostwoID");
            entity.Property(e => e.RodzajId).HasColumnName("RodzajID");
            entity.Property(e => e.UzytkownikId).HasColumnName("UzytkownikID");

            entity.HasOne(d => d.Rodzaj).WithMany(p => p.Czlonkostwas)
                .HasForeignKey(d => d.RodzajId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Czlonkost__Rodza__4F7CD00D");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.Czlonkostwas)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Czlonkost__Uzytk__4E88ABD4");
        });

        modelBuilder.Entity<HarmonogramZajec>(entity =>
        {
            entity.HasKey(e => e.HarmonogramId).HasName("PK__Harmonog__E14DCB0E546137E3");

            entity.ToTable("HarmonogramZajec");

            entity.Property(e => e.HarmonogramId).HasColumnName("HarmonogramID");
            entity.Property(e => e.Data).HasColumnType("datetime");
            entity.Property(e => e.ZajeciaId).HasColumnName("ZajeciaID");

            entity.HasOne(d => d.Zajecia).WithMany(p => p.HarmonogramZajecs)
                .HasForeignKey(d => d.ZajeciaId)
                .HasConstraintName("FK__Harmonogr__Zajec__60A75C0F");
        });

        modelBuilder.Entity<HarmonogramyTrenerow>(entity =>
        {
            entity.HasKey(e => e.HarmonogramId).HasName("PK__Harmonog__E14DCB0E9D336774");

            entity.ToTable("HarmonogramyTrenerow");

            entity.Property(e => e.HarmonogramId).HasColumnName("HarmonogramID");
            entity.Property(e => e.Data).HasColumnType("datetime");
            entity.Property(e => e.TrenerId).HasColumnName("TrenerID");

            entity.HasOne(d => d.Trener).WithMany(p => p.HarmonogramyTrenerows)
                .HasForeignKey(d => d.TrenerId)
                .HasConstraintName("FK__Harmonogr__Trene__6FE99F9F");
        });

        modelBuilder.Entity<HistoriaCzlonkostwa>(entity =>
        {
            entity.HasKey(e => e.HistoriaId).HasName("PK__Historia__0F6B1111F628A157");

            entity.ToTable("HistoriaCzlonkostwa");

            entity.Property(e => e.HistoriaId).HasColumnName("HistoriaID");
            entity.Property(e => e.CzlonkostwoId).HasColumnName("CzlonkostwoID");
            entity.Property(e => e.DataZmiany)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Opis).HasMaxLength(255);

            entity.HasOne(d => d.Czlonkostwo).WithMany(p => p.HistoriaCzlonkostwas)
                .HasForeignKey(d => d.CzlonkostwoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HistoriaC__Czlon__534D60F1");
        });

        modelBuilder.Entity<Kluby>(entity =>
        {
            entity.HasKey(e => e.KlubId).HasName("PK__Kluby__A0FD579B8EB511A9");

            entity.ToTable("Kluby");

            entity.Property(e => e.KlubId).HasColumnName("KlubID");
            entity.Property(e => e.Adres).HasMaxLength(255);
            entity.Property(e => e.Nazwa).HasMaxLength(100);

            entity.HasMany(d => d.Treners).WithMany(p => p.Klubs)
                .UsingEntity<Dictionary<string, object>>(
                    "KlubyTrenerow",
                    r => r.HasOne<Trenerzy>().WithMany()
                        .HasForeignKey("TrenerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__KlubyTren__Trene__75A278F5"),
                    l => l.HasOne<Kluby>().WithMany()
                        .HasForeignKey("KlubId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__KlubyTren__KlubI__74AE54BC"),
                    j =>
                    {
                        j.HasKey("KlubId", "TrenerId").HasName("PK__KlubyTre__9A434D88EE65F9BA");
                        j.ToTable("KlubyTrenerow");
                        j.IndexerProperty<int>("KlubId").HasColumnName("KlubID");
                        j.IndexerProperty<int>("TrenerId").HasColumnName("TrenerID");
                    });
        });

        modelBuilder.Entity<LogiSystemowe>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__LogiSyst__5E5499A85121A8F0");

            entity.ToTable("LogiSystemowe");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Data)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Opis).HasMaxLength(255);
        });

        modelBuilder.Entity<MetodyPlatnosci>(entity =>
        {
            entity.HasKey(e => e.MetodaId).HasName("PK__MetodyPl__48FC0EACCD91CDEA");

            entity.ToTable("MetodyPlatnosci");

            entity.Property(e => e.MetodaId).HasColumnName("MetodaID");
            entity.Property(e => e.Nazwa).HasMaxLength(50);
        });

        modelBuilder.Entity<Opinie>(entity =>
        {
            entity.HasKey(e => e.OpiniaId).HasName("PK__Opinie__6F9FEB93DDAA0D18");

            entity.ToTable("Opinie");

            entity.Property(e => e.OpiniaId).HasColumnName("OpiniaID");
            entity.Property(e => e.Tresc).HasMaxLength(255);
            entity.Property(e => e.UzytkownikId).HasColumnName("UzytkownikID");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.Opinies)
                .HasForeignKey(d => d.UzytkownikId)
                .HasConstraintName("FK__Opinie__Uzytkown__07C12930");
        });

        modelBuilder.Entity<PlanyTreningowe>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__PlanyTre__755C22D781C490C0");

            entity.ToTable("PlanyTreningowe");

            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
            entity.Property(e => e.Opis).HasMaxLength(255);
        });

        modelBuilder.Entity<Platnosci>(entity =>
        {
            entity.HasKey(e => e.PlatnoscId).HasName("PK__Platnosc__AB9E39AA249F8638");

            entity.ToTable("Platnosci");

            entity.Property(e => e.PlatnoscId).HasColumnName("PlatnoscID");
            entity.Property(e => e.CzlonkostwoId).HasColumnName("CzlonkostwoID");
            entity.Property(e => e.DataPlatnosci)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Kwota).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MetodaId).HasColumnName("MetodaID");

            entity.HasOne(d => d.Czlonkostwo).WithMany(p => p.Platnoscis)
                .HasForeignKey(d => d.CzlonkostwoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Platnosci__Czlon__59063A47");

            entity.HasOne(d => d.Metoda).WithMany(p => p.Platnoscis)
                .HasForeignKey(d => d.MetodaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Platnosci__Metod__59FA5E80");
        });

        modelBuilder.Entity<PostepyUzytkownika>(entity =>
        {
            entity.HasKey(e => e.PostepId).HasName("PK__PostepyU__B3CE89198F5CFFB5");

            entity.ToTable("PostepyUzytkownika");

            entity.Property(e => e.PostepId).HasColumnName("PostepID");
            entity.Property(e => e.Opis).HasMaxLength(255);
            entity.Property(e => e.UzytkownikId).HasColumnName("UzytkownikID");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.PostepyUzytkownikas)
                .HasForeignKey(d => d.UzytkownikId)
                .HasConstraintName("FK__PostepyUz__Uzytk__6B24EA82");
        });

        modelBuilder.Entity<Rabaty>(entity =>
        {
            entity.HasKey(e => e.RabatId).HasName("PK__Rabaty__0D0A228B44840ECB");

            entity.ToTable("Rabaty");

            entity.Property(e => e.RabatId).HasColumnName("RabatID");
            entity.Property(e => e.Nazwa).HasMaxLength(50);
        });

        modelBuilder.Entity<Raporty>(entity =>
        {
            entity.HasKey(e => e.RaportId).HasName("PK__Raporty__234C4E1B1FEC8BC8");

            entity.ToTable("Raporty");

            entity.Property(e => e.RaportId).HasColumnName("RaportID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
        });

        modelBuilder.Entity<RejestracjeNaZajecium>(entity =>
        {
            entity.HasKey(e => e.RejestracjaId).HasName("PK__Rejestra__91E27E7406304837");

            entity.Property(e => e.RejestracjaId).HasColumnName("RejestracjaID");
            entity.Property(e => e.HarmonogramId).HasColumnName("HarmonogramID");
            entity.Property(e => e.UzytkownikId).HasColumnName("UzytkownikID");

            entity.HasOne(d => d.Harmonogram).WithMany(p => p.RejestracjeNaZajecia)
                .HasForeignKey(d => d.HarmonogramId)
                .HasConstraintName("FK__Rejestrac__Harmo__6383C8BA");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.RejestracjeNaZajecia)
                .HasForeignKey(d => d.UzytkownikId)
                .HasConstraintName("FK__Rejestrac__Uzytk__6477ECF3");
        });

        modelBuilder.Entity<RodzajeCzlonkostwa>(entity =>
        {
            entity.HasKey(e => e.RodzajId).HasName("PK__RodzajeC__19A2F39D81CC41A1");

            entity.ToTable("RodzajeCzlonkostwa");

            entity.Property(e => e.RodzajId).HasColumnName("RodzajID");
            entity.Property(e => e.Cena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Nazwa).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolaId).HasName("PK__Role__8DF280FBED050F7E");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Nazwa, "UQ__Role__602223FFC0EBFFA5").IsUnique();

            entity.Property(e => e.RolaId).HasColumnName("RolaID");
            entity.Property(e => e.Nazwa).HasMaxLength(50);

            entity.HasMany(d => d.Uprawnienies).WithMany(p => p.Rolas)
                .UsingEntity<Dictionary<string, object>>(
                    "UprawnieniaRol",
                    r => r.HasOne<Uprawnienium>().WithMany()
                        .HasForeignKey("UprawnienieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Uprawnien__Upraw__4316F928"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RolaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Uprawnien__RolaI__4222D4EF"),
                    j =>
                    {
                        j.HasKey("RolaId", "UprawnienieId").HasName("PK__Uprawnie__E930E32C1761F162");
                        j.ToTable("UprawnieniaRol");
                        j.IndexerProperty<int>("RolaId").HasColumnName("RolaID");
                        j.IndexerProperty<int>("UprawnienieId").HasColumnName("UprawnienieID");
                    });
        });

        modelBuilder.Entity<Sprzet>(entity =>
        {
            entity.HasKey(e => e.SprzetId).HasName("PK__Sprzet__B7888C95AD624CDA");

            entity.ToTable("Sprzet");

            entity.Property(e => e.SprzetId).HasColumnName("SprzetID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
        });

        modelBuilder.Entity<StatusSprzetu>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__StatusSp__C8EE2043CA3CE492");

            entity.ToTable("StatusSprzetu");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.SprzetId).HasColumnName("SprzetID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Sprzet).WithMany(p => p.StatusSprzetus)
                .HasForeignKey(d => d.SprzetId)
                .HasConstraintName("FK__StatusSpr__Sprze__7A672E12");
        });

        modelBuilder.Entity<SzablonyRaportow>(entity =>
        {
            entity.HasKey(e => e.SzablonId).HasName("PK__Szablony__40D35DF104006F35");

            entity.ToTable("SzablonyRaportow");

            entity.Property(e => e.SzablonId).HasColumnName("SzablonID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
        });

        modelBuilder.Entity<Trenerzy>(entity =>
        {
            entity.HasKey(e => e.TrenerId).HasName("PK__Trenerzy__ABE1A132345487B5");

            entity.ToTable("Trenerzy");

            entity.Property(e => e.TrenerId).HasColumnName("TrenerID");
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
        });

        modelBuilder.Entity<Uprawnienium>(entity =>
        {
            entity.HasKey(e => e.UprawnienieId).HasName("PK__Uprawnie__4C263D771EF25350");

            entity.HasIndex(e => e.Nazwa, "UQ__Uprawnie__602223FFB3898074").IsUnique();

            entity.Property(e => e.UprawnienieId).HasColumnName("UprawnienieID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
        });

        modelBuilder.Entity<UstawieniaSystemu>(entity =>
        {
            entity.HasKey(e => e.UstawienieId).HasName("PK__Ustawien__230D536F45891CC0");

            entity.ToTable("UstawieniaSystemu");

            entity.Property(e => e.UstawienieId).HasColumnName("UstawienieID");
            entity.Property(e => e.Klucz).HasMaxLength(100);
            entity.Property(e => e.Wartosc).HasMaxLength(255);
        });

        modelBuilder.Entity<UstawieniaUzytkownika>(entity =>
        {
            entity.HasKey(e => e.UstawienieId).HasName("PK__Ustawien__230D536FAEE86BA8");

            entity.ToTable("UstawieniaUzytkownika");

            entity.Property(e => e.UstawienieId).HasColumnName("UstawienieID");
            entity.Property(e => e.Klucz).HasMaxLength(100);
            entity.Property(e => e.UzytkownikId).HasColumnName("UzytkownikID");
            entity.Property(e => e.Wartosc).HasMaxLength(255);

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.UstawieniaUzytkownikas)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ustawieni__Uzytk__49C3F6B7");
        });

        modelBuilder.Entity<Uzytkownicy>(entity =>
        {
            entity.HasKey(e => e.UzytkownikId).HasName("PK__Uzytkown__3E7072736CFA3771");

            entity.ToTable("Uzytkownicy");

            entity.HasIndex(e => e.Email, "UQ__Uzytkown__A9D105349E397662").IsUnique();

            entity.Property(e => e.UzytkownikId).HasColumnName("UzytkownikID");
            entity.Property(e => e.DataUtworzenia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);

            entity.HasMany(d => d.Rolas).WithMany(p => p.Uzytkowniks)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleUzytkownikow",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RolaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RoleUzytk__RolaI__46E78A0C"),
                    l => l.HasOne<Uzytkownicy>().WithMany()
                        .HasForeignKey("UzytkownikId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RoleUzytk__Uzytk__45F365D3"),
                    j =>
                    {
                        j.HasKey("UzytkownikId", "RolaId").HasName("PK__RoleUzyt__96AF5A7CC1908372");
                        j.ToTable("RoleUzytkownikow");
                        j.IndexerProperty<int>("UzytkownikId").HasColumnName("UzytkownikID");
                        j.IndexerProperty<int>("RolaId").HasColumnName("RolaID");
                    });
        });

        modelBuilder.Entity<Wyzwanium>(entity =>
        {
            entity.HasKey(e => e.WyzwanieId).HasName("PK__Wyzwania__BCA21DBDA9D84EF7");

            entity.Property(e => e.WyzwanieId).HasColumnName("WyzwanieID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
            entity.Property(e => e.Opis).HasMaxLength(255);
        });

        modelBuilder.Entity<Zajecium>(entity =>
        {
            entity.HasKey(e => e.ZajeciaId).HasName("PK__Zajecia__D20A24FB71FE33BA");

            entity.Property(e => e.ZajeciaId).HasColumnName("ZajeciaID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
            entity.Property(e => e.Opis).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
