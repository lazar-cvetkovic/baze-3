using System;

namespace Baze3.Domain
{
    public sealed class PoslovniZadatak
    {
        public int RbIzvestaja { get; set; }
        public int IDZadatka { get; set; }
        public string NazivZadatka { get; set; } = string.Empty;
        public string OpisAktivnosti { get; set; }
        public TimeSpan VremeUSatima { get; set; }
        public byte VrstaPosla { get; set; } = 1;
    }
}
