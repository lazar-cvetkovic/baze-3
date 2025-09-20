namespace Baze3.Domain
{
    public sealed class Adresa
    {
        public int RbAdrese { get; set; }
        public string BrojStana { get; set; }
        public string Ulica { get; set; } = string.Empty;
        public int RbMesta { get; set; }
    }
}
