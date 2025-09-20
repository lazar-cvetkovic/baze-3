using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baze3.Repositories
{
    public sealed class InMemoryIzvestajRepository : IIzvestajRepository
    {
        private readonly List<IzvestajZaposlenog> _store = new List<IzvestajZaposlenog>();

        public IEnumerable<IzvestajZaposlenog> GetAll() => _store.ToList();

        public IEnumerable<IzvestajZaposlenog> Search(string q)
        {
            var s = (q ?? string.Empty).Trim();
            return _store.Where(x => x.Ime.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 || x.Prezime.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 || x.MaticniBrojZaposlenog.Contains(s));
        }

        public void Add(IzvestajZaposlenog iz)
        {
            if (_store.Any(x => x.RbIzvestaja == iz.RbIzvestaja)) { throw new InvalidOperationException("Izveštaj već postoji."); }
            _store.Add(Clone(iz));
        }

        public void Update(IzvestajZaposlenog iz)
        {
            var idx = _store.FindIndex(x => x.RbIzvestaja == iz.RbIzvestaja); if (idx < 0) { throw new InvalidOperationException("Izveštaj ne postoji."); }
            _store[idx] = Clone(iz);
        }

        private static IzvestajZaposlenog Clone(IzvestajZaposlenog s) => new IzvestajZaposlenog { RbIzvestaja = s.RbIzvestaja, UkupnoRadnoVremeNaIstrazivanjuIRazvoju = s.UkupnoRadnoVremeNaIstrazivanjuIRazvoju, UkupnoRadnoVreme = s.UkupnoRadnoVreme, Ime = s.Ime, Prezime = s.Prezime, OpisAktivnosti = s.OpisAktivnosti, Napomena = s.Napomena, MaticniBrojZaposlenog = s.MaticniBrojZaposlenog };
    }
}
