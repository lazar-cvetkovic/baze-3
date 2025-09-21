using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Repositories
{
    public interface IMestoRepository
    {
        IEnumerable<Mesto> GetByOpstina(int rbOpstine);
        bool Exists(int rbMesta);
        void Add(Mesto m);
    }
}
