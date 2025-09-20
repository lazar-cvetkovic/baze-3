using Baze3.Domain;
using System.Collections.Generic;

namespace Baze3.Repositories
{
    public interface IUgovorRepository
    {
        IEnumerable<UgovorORadu> GetAll();
        IEnumerable<UgovorORadu> Search(string query);
        void Add(UgovorORadu u);
        void Update(UgovorORadu u);
    }
}
