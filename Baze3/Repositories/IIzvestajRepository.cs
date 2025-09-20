using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Repositories
{
    public interface IIzvestajRepository
    {
        IEnumerable<IzvestajZaposlenog> GetAll();
        IEnumerable<IzvestajZaposlenog> Search(string query);
        void Add(IzvestajZaposlenog iz);
        void Update(IzvestajZaposlenog iz);
    }
}
