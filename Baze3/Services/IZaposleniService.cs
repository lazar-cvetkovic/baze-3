using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Services
{
    public interface IZaposleniService
    {
        IEnumerable<Zaposleni> GetAll();
        IEnumerable<Zaposleni> SearchByIme(string ime);
        IEnumerable<Zaposleni> SearchByPrezime(string prezime);
        void Create(Zaposleni z);
        void Update(Zaposleni z);
        void Delete(string maticniBroj);
    }
}
