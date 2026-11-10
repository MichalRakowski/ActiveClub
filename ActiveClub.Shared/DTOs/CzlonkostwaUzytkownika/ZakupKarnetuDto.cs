using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika
{
    public class ZakupKarnetuDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Użytkownik jest wymagany.")]
        public int UzytkownikId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Klub jest wymagany.")]
        public int KlubId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Rodzaj członkostwa jest wymagany.")]
        public int RodzajCzlonkostwaId { get; set; }
    }
}