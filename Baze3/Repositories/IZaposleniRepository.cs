using Baze3.Domain;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baze3.Repositories
{
    public interface IZaposleniRepository
    {
        IEnumerable<Zaposleni> GetAll();
        IEnumerable<Zaposleni> FindByIme(string ime);
        IEnumerable<Zaposleni> FindByPrezime(string prezime);
        void Add(Zaposleni z);
        void Update(Zaposleni z);
        void Delete(string maticniBrojZaposlenog);
    }
}
