using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Services
{
    public interface IMestoService
    {
        IEnumerable<Mesto> GetByOpstina(int rbOpstine);
        bool Exists(int rbMesta);
        void Create(Mesto m);
    }
}
