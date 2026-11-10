using Microsoft.EntityFrameworkCore;
using ActiveClub.Data.Models;

namespace ActiveClub.Data
{
    public class ActiveClubContext : DbContext
    {
        // Konstruktor przyjmujący DbContextOptions
        public ActiveClubContext(DbContextOptions<ActiveClubContext> options)
            : base(options)
        {
        }

        // DbSet-y odpowiadające tabelom w bazie danych
        public DbSet<Aktualnosc> Aktualnosci { get; set; }
        
    }
}

