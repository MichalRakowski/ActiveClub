using System;

namespace ActiveClub.Data.Models
{
    public class Aktualnosc
    {
        public int Id { get; set; }               // Klucz główny
        public string Tytul { get; set; }         // Tytuł aktualności
        public string Tresc { get; set; }         // Treść aktualności
        public DateTime DataDodania { get; set; } // Data dodania
    }
}
