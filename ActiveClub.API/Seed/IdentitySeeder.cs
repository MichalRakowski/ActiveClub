using ActiveClubData.Models;
using Microsoft.AspNetCore.Identity;

namespace ActiveClub.API.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var roles = new[]
            {
                new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN", Opis = "Administrator systemu", CzyAktywna = true },
                new ApplicationRole { Name = "Pracownik", NormalizedName = "PRACOWNIK", Opis = "Pracownik klubu", CzyAktywna = true },
                new ApplicationRole { Name = "Trener", NormalizedName = "TRENER", Opis = "Trener personalny lub instruktor", CzyAktywna = true },
                new ApplicationRole { Name = "Klient", NormalizedName = "KLIENT", Opis = "Klient klubu", CzyAktywna = true }
            };

            foreach (var role in roles)
            {
                var exists = await roleManager.RoleExistsAsync(role.Name!);
                if (!exists)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}