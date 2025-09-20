using System;

namespace Baze3.Domain
{
    public sealed class ClanUgovora
    {
        public string MaticniBrojZaposlenog { get; set; } = string.Empty;
        public string MaticniBrojPreduzeca { get; set; } = string.Empty;
        public DateTime DatumZakljucivanja { get; set; }
        public int RbClana { get; set; }
        public string TekstClana { get; set; }
    }
}
