using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Services
{
    public interface IAdresaService
    {
        IEnumerable<Adresa> GetAll();
        IEnumerable<Adresa> SearchByUlica(string ulica);
        bool Exists(int rbAdrese);
        void Create(Adresa a);
        void Update(Adresa a);
        Adresa Get(int rbAdrese);
    }
}
