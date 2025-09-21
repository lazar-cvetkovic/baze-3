using Baze3.Database;
using Baze3.Domain;
using System.Collections.Generic;
using System.Data;

namespace Baze3.Repositories.Database
{
    public sealed class DbPreduzeceRepository : IPreduzeceRepository
    {
        private readonly IDatabase _db;

        public DbPreduzeceRepository(IDatabase db) => _db = db;

        public IEnumerable<Preduzece> GetAll()
        {
            const string sql = "SELECT MaticniBrojPreduzeca,Naziv,PIB FROM dbo.Preduzece ORDER BY Naziv";
            foreach (var r in _db.Query(sql)) 
                yield return Map(r);
        }

        public IEnumerable<Preduzece> FindByNaziv(string naziv)
        {
            const string sql = "SELECT MaticniBrojPreduzeca,Naziv,PIB FROM dbo.Preduzece WHERE Naziv LIKE @q ORDER BY Naziv";
            foreach (var r in _db.Query(sql, p => p.AddWithValue("@q", DatabaseUtils.Like(naziv)))) 
                yield return Map(r);
        }

        public void Add(Preduzece p)
        {
            _db.Execute("INSERT INTO dbo.Preduzece(MaticniBrojPreduzeca,Naziv,PIB) VALUES(@mbr,@naziv,@pib)", pr =>
            {
                pr.AddWithValue("@mbr", p.MaticniBrojPreduzeca);
                pr.AddWithValue("@naziv", p.Naziv);
                pr.AddWithValue("@pib", p.PIB);
            });
        }

        public void Update(Preduzece p)
        {
            _db.Execute("UPDATE dbo.Preduzece SET Naziv=@naziv,PIB=@pib WHERE MaticniBrojPreduzeca=@mbr", pr =>
            {
                pr.AddWithValue("@mbr", p.MaticniBrojPreduzeca);
                pr.AddWithValue("@naziv", p.Naziv);
                pr.AddWithValue("@pib", p.PIB);
            });
        }

        public void Delete(string mbr)
            => _db.Execute("DELETE FROM dbo.Preduzece WHERE MaticniBrojPreduzeca=@mbr", p => p.AddWithValue("@mbr", mbr));

        private static Preduzece Map(IDataRecord r) => new Preduzece
        {
            MaticniBrojPreduzeca = DatabaseUtils.GetString(r, "MaticniBrojPreduzeca"),
            Naziv = DatabaseUtils.GetString(r, "Naziv"),
            PIB = DatabaseUtils.GetString(r, "PIB")
        };
    }
}