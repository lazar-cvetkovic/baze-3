using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baze3.Repositories
{
    public sealed class InMemoryPreduzeceRepository : IPreduzeceRepository
    {
        private readonly List<Preduzece> _store = new List<Preduzece>();

        public IEnumerable<Preduzece> GetAll() => _store.ToList();

        public IEnumerable<Preduzece> FindByNaziv(string naziv)
        {
            var q = (naziv ?? string.Empty).Trim();
            return _store.Where(x => x.Naziv.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void Add(Preduzece p)
        {
            if (_store.Any(x => x.MaticniBrojPreduzeca == p.MaticniBrojPreduzeca))
            {
                throw new InvalidOperationException("Preduzeće već postoji.");
            }
            _store.Add(Clone(p));
        }

        public void Update(Preduzece p)
        {
            var idx = _store.FindIndex(x => x.MaticniBrojPreduzeca == p.MaticniBrojPreduzeca);
            if (idx < 0)
            {
                throw new InvalidOperationException("Preduzeće ne postoji.");
            }
            _store[idx] = Clone(p);
        }

        public void Delete(string mbr)
        {
            var obj = _store.FirstOrDefault(x => x.MaticniBrojPreduzeca == mbr);
            if (obj != null) { _store.Remove(obj); }
        }

        private static Preduzece Clone(Preduzece p) => new Preduzece { MaticniBrojPreduzeca = p.MaticniBrojPreduzeca, Naziv = p.Naziv, PIB = p.PIB };
    }
}
