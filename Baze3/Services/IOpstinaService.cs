using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Services
{
    public interface IOpstinaService
    {
        IEnumerable<Opstina> GetAll();
        bool Exists(int rb);
        void Create(Opstina o);
    }
}
