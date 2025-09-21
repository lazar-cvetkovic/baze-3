using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Repositories
{
    public interface IAdresaRepository
    {
        IEnumerable<Adresa> GetAll();
        IEnumerable<Adresa> FindByUlica(string ulica);
        bool Exists(int rbAdrese);
        void Add(Adresa a);
        void Update(Adresa a);
        Adresa Get(int rbAdrese);
    }
}
