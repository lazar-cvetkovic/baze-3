using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Services
{
    public interface IIzvestajiService
    {
        IEnumerable<IzvestajZaposlenog> GetAll();
        IEnumerable<IzvestajZaposlenog> Search(string query);
        void Create(IzvestajZaposlenog iz);
        void Update(IzvestajZaposlenog iz);
        byte[] GeneratePdf(IzvestajZaposlenog iz);
    }
}
