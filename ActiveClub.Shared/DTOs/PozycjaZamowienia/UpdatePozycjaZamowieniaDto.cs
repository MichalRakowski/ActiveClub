using System.ComponentModel.DataAnnotations;

namespace ActiveClub.Shared.DTOs.PozycjeZamowienia
{
    public class UpdatePozycjaZamowieniaDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Zamówienie jest wymagane.")]
        public int ZamowienieId { get; set; }

        [Required(ErrorMessage = "Typ pozycji jest wymagany.")]
        [StringLength(40, ErrorMessage = "Typ może mieć maksymalnie 40 znaków.")]
        public string Typ { get; set; } = string.Empty;

        public int? RefId { get; set; }

        [Required(ErrorMessage = "Nazwa pozycji jest wymagana.")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Nazwa musi mieć od 2 do 200 znaków.")]
        public string Nazwa { get; set; } = string.Empty;

        [StringLength(1000,
            ErrorMessage = "Opis może mieć maksymalnie 1000 znaków.")]
        public string? Opis { get; set; }

        [Range(1, 10000,
            ErrorMessage = "Ilość musi być większa od 0.")]
        public int Ilosc { get; set; }

        [Range(0, 999999,
            ErrorMessage = "Cena jednostkowa nie może być ujemna.")]
        public decimal CenaJedn { get; set; }

        [Range(0, 999999,
            ErrorMessage = "Kwota brutto nie może być ujemna.")]
        public decimal KwotaBrutto { get; set; }
    }
}