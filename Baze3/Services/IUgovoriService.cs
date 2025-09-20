using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Services
{
    public interface IUgovoriService
    {
        IEnumerable<UgovorORadu> GetAll();
        IEnumerable<UgovorORadu> Search(string query);
        void Create(UgovorORadu u);
        void Update(UgovorORadu u);
        byte[] GeneratePdf(UgovorORadu u);
    }
}
