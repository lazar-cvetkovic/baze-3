using Baze3.Domain;
using Baze3.Repositories;
using System.Collections.Generic;
using System.Text;

namespace Baze3.Services
{
    public sealed class UgovoriService : IUgovoriService
    {
        private readonly IUgovorRepository _repo;

        public UgovoriService(IUgovorRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<UgovorORadu> GetAll() => _repo.GetAll();
        public IEnumerable<UgovorORadu> Search(string query) => string.IsNullOrWhiteSpace(query) ? _repo.GetAll() : _repo.Search(query);
        public void Create(UgovorORadu u) => _repo.Add(u);
        public void Update(UgovorORadu u) => _repo.Update(u);

        public byte[] GeneratePdf(UgovorORadu u)
        {
            var sb = new StringBuilder();

            sb.AppendLine("UGOVOR O RADU");
            sb.AppendLine("================");
            sb.AppendLine("Naziv: " + u.Naziv);
            sb.AppendLine("Zaposleni: " + u.MaticniBrojZaposlenog);
            sb.AppendLine("Preduzeće: " + u.MaticniBrojPreduzeca);
            sb.AppendLine("Datum: " + u.DatumZakljucivanja.ToString("yyyy-MM-dd"));
            sb.AppendLine("Aktivan: " + u.Aktivan);

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
