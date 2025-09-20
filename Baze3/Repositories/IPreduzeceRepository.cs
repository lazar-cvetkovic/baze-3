using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Repositories
{
    public interface IPreduzeceRepository
    {
        IEnumerable<Preduzece> GetAll();
        IEnumerable<Preduzece> FindByNaziv(string naziv);
        void Add(Preduzece p);
        void Update(Preduzece p);
        void Delete(string maticniBrojPreduzeca);
    }
}
