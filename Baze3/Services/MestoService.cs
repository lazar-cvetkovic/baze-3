using Baze3.Domain;
using Baze3.Repositories;
using System.Collections.Generic;

namespace Baze3.Services
{
    public sealed class MestoService : IMestoService
    {
        private readonly IMestoRepository _repo;
        public MestoService(IMestoRepository repo) { _repo = repo; }
        public IEnumerable<Mesto> GetByOpstina(int rb) => _repo.GetByOpstina(rb);
        public bool Exists(int rbMesta) => _repo.Exists(rbMesta);
        public void Create(Mesto m) => _repo.Add(m);
    }
}
