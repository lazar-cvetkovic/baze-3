using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Services
{
    public interface IPreduzecaService
    {
        IEnumerable<Preduzece> GetAll();
        IEnumerable<Preduzece> SearchByNaziv(string naziv);
        void Create(Preduzece p);
        void Update(Preduzece p);
        void Delete(string maticniBroj);
    }
}
