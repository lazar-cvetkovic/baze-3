using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baze3.Repositories
{
    public sealed class InMemoryUgovorRepository : IUgovorRepository
    {
        private readonly List<UgovorORadu> _store = new List<UgovorORadu>();

        public IEnumerable<UgovorORadu> GetAll() => _store.ToList();

        public IEnumerable<UgovorORadu> Search(string q)
        {
            var s = (q ?? string.Empty).Trim();
            return _store.Where(x => x.Naziv.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 || x.MaticniBrojZaposlenog.Contains(s) || x.MaticniBrojPreduzeca.Contains(s));
        }

        public void Add(UgovorORadu u)
        {
            if (_store.Any(x => SameKey(x, u)))
            {
                throw new InvalidOperationException("Ugovor već postoji.");
            }
            _store.Add(Clone(u));
        }

        public void Update(UgovorORadu u)
        {
            var idx = _store.FindIndex(x => SameKey(x, u)); if (idx < 0)
            {
                throw new InvalidOperationException("Ugovor ne postoji.");
            }
            _store[idx] = Clone(u);
        }

        private static bool SameKey(UgovorORadu a, UgovorORadu b) => a.MaticniBrojZaposlenog == b.MaticniBrojZaposlenog && a.MaticniBrojPreduzeca == b.MaticniBrojPreduzeca && a.DatumZakljucivanja.Date == b.DatumZakljucivanja.Date;
        
        private static UgovorORadu Clone(UgovorORadu u) => new UgovorORadu { MaticniBrojZaposlenog = u.MaticniBrojZaposlenog, MaticniBrojPreduzeca = u.MaticniBrojPreduzeca, DatumZakljucivanja = u.DatumZakljucivanja, Aktivan = u.Aktivan, Naziv = u.Naziv };
    }
}
