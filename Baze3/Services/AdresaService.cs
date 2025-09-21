using Baze3.Domain;
using Baze3.Repositories;
using System.Collections.Generic;

namespace Baze3.Services
{
    public sealed class AdresaService : IAdresaService
    {
        private readonly IAdresaRepository _repo;
        public AdresaService(IAdresaRepository repo) { _repo = repo; }

        public IEnumerable<Adresa> GetAll() => _repo.GetAll();
        public IEnumerable<Adresa> SearchByUlica(string ulica) => _repo.FindByUlica(ulica);
        public bool Exists(int rbAdrese) => _repo.Exists(rbAdrese);
        public void Create(Adresa a) => _repo.Add(a);
        public void Update(Adresa a) => _repo.Update(a);
        public Adresa Get(int rbAdrese) => _repo.Get(rbAdrese);
    }
}
