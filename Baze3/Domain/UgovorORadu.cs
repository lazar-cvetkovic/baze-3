using System;

namespace Baze3.Domain
{
    public sealed class UgovorORadu
    {
        public string MaticniBrojZaposlenog { get; set; } = string.Empty;
        public string MaticniBrojPreduzeca { get; set; } = string.Empty;
        public DateTime DatumZakljucivanja { get; set; }
        public string Aktivan { get; set; } = "ne";
        public string Naziv { get; set; } = string.Empty;
    }
}
