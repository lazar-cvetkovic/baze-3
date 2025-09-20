using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baze3.Repositories
{
    public sealed class InMemoryZaposleniRepository : IZaposleniRepository
    {
        private readonly List<Zaposleni> _store = new List<Zaposleni>();

        public IEnumerable<Zaposleni> GetAll()
        {
            return _store.ToList();
        }

        public IEnumerable<Zaposleni> FindByIme(string ime)
        {
            var q = (ime ?? string.Empty).Trim();
            return _store.Where(x => x.Ime.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public IEnumerable<Zaposleni> FindByPrezime(string prezime)
        {
            var q = (prezime ?? string.Empty).Trim();
            return _store.Where(x => x.Prezime.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void Add(Zaposleni z)
        {
            if (_store.Any(x => x.MaticniBrojZaposlenog == z.MaticniBrojZaposlenog))
            {
                throw new InvalidOperationException("Zaposleni već postoji.");
            }
            _store.Add(Clone(z));
        }

        public void Update(Zaposleni z)
        {
            var idx = _store.FindIndex(x => x.MaticniBrojZaposlenog == z.MaticniBrojZaposlenog);
            if (idx < 0)
            {
                throw new InvalidOperationException("Zaposleni ne postoji.");
            }
            _store[idx] = Clone(z);
        }

        public void Delete(string mbr)
        {
            var obj = _store.FirstOrDefault(x => x.MaticniBrojZaposlenog == mbr);
            if (obj != null)
            {
                _store.Remove(obj);
            }
        }

        private static Zaposleni Clone(Zaposleni s) => new Zaposleni { MaticniBrojZaposlenog = s.MaticniBrojZaposlenog, BrojLicneKarte = s.BrojLicneKarte, Ime = s.Ime, Prezime = s.Prezime, StrucnaSprema = s.StrucnaSprema, Pozicija = s.Pozicija, RbAdrese = s.RbAdrese, NazivOpstine = s.NazivOpstine };
    }
}
