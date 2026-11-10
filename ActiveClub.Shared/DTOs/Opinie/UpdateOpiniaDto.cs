using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.Opinie
{
    public class UpdateOpiniaDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Klub jest wymagany.")]
        public int KlubId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Użytkownik jest wymagany.")]
        public int UzytkownikId { get; set; }

        [Range(1, 5,
            ErrorMessage = "Ocena musi być w zakresie od 1 do 5.")]
        public byte Ocena { get; set; }

        [StringLength(1000,
            ErrorMessage = "Komentarz nie może mieć więcej niż 1000 znaków.")]
        public string? Komentarz { get; set; }

        public bool CzyWidoczna { get; set; }

        public DateTime DataDodania { get; set; }
    }
}