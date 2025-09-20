namespace Baze3.Domain
{
    public sealed class Zaposleni
    {
        public string MaticniBrojZaposlenog { get; set; } = string.Empty;
        public string BrojLicneKarte { get; set; } = string.Empty;
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string StrucnaSprema { get; set; }
        public string Pozicija { get; set; }
        public int RbAdrese { get; set; }
        public string NazivOpstine { get; set; } = string.Empty;
    }
}
