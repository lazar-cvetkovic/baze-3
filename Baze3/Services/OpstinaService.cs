using Baze3.Domain;
using Baze3.Repositories;
using System.Collections.Generic;

namespace Baze3.Services
{
    public sealed class OpstinaService : IOpstinaService
    {
        private readonly IOpstinaRepository _repo;
        public OpstinaService(IOpstinaRepository repo) { _repo = repo; }
        public IEnumerable<Opstina> GetAll() => _repo.GetAll();
        public bool Exists(int rb) => _repo.Exists(rb);
        public void Create(Opstina o) => _repo.Add(o);
    }
}
