using System;

namespace Baze3.Domain
{
    public sealed class IzvestajZaposlenog
    {
        public int RbIzvestaja { get; set; }
        public TimeSpan UkupnoRadnoVremeNaIstrazivanjuIRazvoju { get; set; }
        public TimeSpan UkupnoRadnoVreme { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string OpisAktivnosti { get; set; }
        public string Napomena { get; set; }
        public string MaticniBrojZaposlenog { get; set; } = string.Empty;
    }
}
