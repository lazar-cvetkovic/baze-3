using Baze3.Domain;
using Baze3.Repositories;
using System.Collections.Generic;

namespace Baze3.Services
{
    public sealed class PreduzecaService : IPreduzecaService
    {
        private readonly IPreduzeceRepository _repo;

        public PreduzecaService(IPreduzeceRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Preduzece> GetAll() => _repo.GetAll();
        public IEnumerable<Preduzece> SearchByNaziv(string naziv) => string.IsNullOrWhiteSpace(naziv) ? _repo.GetAll() : _repo.FindByNaziv(naziv);
        public void Create(Preduzece p) => _repo.Add(p);
        public void Update(Preduzece p) => _repo.Update(p);
        public void Delete(string maticniBroj) => _repo.Delete(maticniBroj);
    }
}
