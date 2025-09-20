using Baze3.Domain;
using Baze3.Repositories;
using System.Collections.Generic;

namespace Baze3.Services
{
    public sealed class ZaposleniService : IZaposleniService
    {
        private readonly IZaposleniRepository _repo;

        public ZaposleniService(IZaposleniRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Zaposleni> GetAll() => _repo.GetAll();
        public IEnumerable<Zaposleni> SearchByIme(string ime) => string.IsNullOrWhiteSpace(ime) ? _repo.GetAll() : _repo.FindByIme(ime);
        public IEnumerable<Zaposleni> SearchByPrezime(string prezime) => string.IsNullOrWhiteSpace(prezime) ? _repo.GetAll() : _repo.FindByPrezime(prezime);
        public void Create(Zaposleni z) => _repo.Add(z);
        public void Update(Zaposleni z) => _repo.Update(z);
        public void Delete(string maticniBroj) => _repo.Delete(maticniBroj);
    }
}
