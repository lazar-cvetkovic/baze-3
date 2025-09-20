using Baze3.Domain;
using Baze3.Repositories;
using System.Collections.Generic;
using System.Text;

namespace Baze3.Services
{
    public sealed class IzvestajiService : IIzvestajiService
    {
        private readonly IIzvestajRepository _repo;

        public IzvestajiService(IIzvestajRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<IzvestajZaposlenog> GetAll() => _repo.GetAll();
        public IEnumerable<IzvestajZaposlenog> Search(string query) => string.IsNullOrWhiteSpace(query) ? _repo.GetAll() : _repo.Search(query);
        public void Create(IzvestajZaposlenog iz) => _repo.Add(iz);
        public void Update(IzvestajZaposlenog iz) => _repo.Update(iz);

        public byte[] GeneratePdf(IzvestajZaposlenog iz)
        {
            var sb = new StringBuilder();

            sb.AppendLine("IZVEŠTAJ ZAPOSLENOG");
            sb.AppendLine("===================");
            sb.AppendLine("Rb: " + iz.RbIzvestaja);
            sb.AppendLine("Zaposleni: " + iz.Ime + " " + iz.Prezime + " (" + iz.MaticniBrojZaposlenog + ")");
            sb.AppendLine("I&R vreme: " + iz.UkupnoRadnoVremeNaIstrazivanjuIRazvoju);
            sb.AppendLine("Ukupno vreme: " + iz.UkupnoRadnoVreme);
            sb.AppendLine("Opis: " + iz.OpisAktivnosti);
            sb.AppendLine("Napomena: " + iz.Napomena);

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
