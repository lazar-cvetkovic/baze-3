using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Repositories
{
    public interface IOpstinaRepository
    {
        IEnumerable<Opstina> GetAll();
        bool Exists(int rb);
        void Add(Opstina o);
    }
}
