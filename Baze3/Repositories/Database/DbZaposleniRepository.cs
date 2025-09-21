using Baze3.Database;
using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Data;

namespace Baze3.Repositories.Database
{
    public sealed class DbZaposleniRepository : IZaposleniRepository
    {
        private readonly IDatabase _db;

        public DbZaposleniRepository(IDatabase db) => _db = db;

        public IEnumerable<Zaposleni> GetAll()
        {
            const string sql = "SELECT MaticniBrojZaposlenog,BrojLicneKarte,Ime,Prezime,StrucnaSprema,Pozicija,RbAdrese,NazivOpstine FROM dbo.Zaposleni ORDER BY Prezime,Ime";

            foreach (var r in _db.Query(sql))
                yield return Map(r);
        }

        public IEnumerable<Zaposleni> FindByIme(string ime)
        {
            const string sql = "SELECT MaticniBrojZaposlenog,BrojLicneKarte,Ime,Prezime,StrucnaSprema,Pozicija,RbAdrese,NazivOpstine FROM dbo.Zaposleni WHERE Ime LIKE @q ORDER BY Prezime,Ime";

            foreach (var r in _db.Query(sql, p => p.AddWithValue("@q", DatabaseUtils.Like(ime))))
                yield return Map(r);
        }

        public IEnumerable<Zaposleni> FindByPrezime(string prezime)
        {
            const string sql = "SELECT MaticniBrojZaposlenog,BrojLicneKarte,Ime,Prezime,StrucnaSprema,Pozicija,RbAdrese,NazivOpstine FROM dbo.Zaposleni WHERE Prezime LIKE @q ORDER BY Prezime,Ime";

            foreach (var r in _db.Query(sql, p => p.AddWithValue("@q", DatabaseUtils.Like(prezime))))
                yield return Map(r);
        }

        public void Add(Zaposleni z)
        {
            const string sql = @"INSERT INTO dbo.Zaposleni(MaticniBrojZaposlenog,BrojLicneKarte,Ime,Prezime,StrucnaSprema,Pozicija,RbAdrese,NazivOpstine)
                                 VALUES(@mbr,@lk,@ime,@prezime,@ss,@poz,@rb,@opst)";
            _db.Execute(sql, p =>
            {
                p.AddWithValue("@mbr", z.MaticniBrojZaposlenog);
                p.AddWithValue("@lk", z.BrojLicneKarte);
                p.AddWithValue("@ime", z.Ime);
                p.AddWithValue("@prezime", z.Prezime);
                p.AddWithValue("@ss", (object)z.StrucnaSprema ?? DBNull.Value);
                p.AddWithValue("@poz", (object)z.Pozicija ?? DBNull.Value);
                p.AddWithValue("@rb", z.RbAdrese);
                p.AddWithValue("@opst", z.NazivOpstine);
            });
        }

        public void Update(Zaposleni z)
        {
            const string sql = @"UPDATE dbo.Zaposleni SET BrojLicneKarte=@lk,Ime=@ime,Prezime=@prezime,StrucnaSprema=@ss,Pozicija=@poz,RbAdrese=@rb,NazivOpstine=@opst
                                 WHERE MaticniBrojZaposlenog=@mbr";
            _db.Execute(sql, p =>
            {
                p.AddWithValue("@mbr", z.MaticniBrojZaposlenog);
                p.AddWithValue("@lk", z.BrojLicneKarte);
                p.AddWithValue("@ime", z.Ime);
                p.AddWithValue("@prezime", z.Prezime);
                p.AddWithValue("@ss", (object)z.StrucnaSprema ?? DBNull.Value);
                p.AddWithValue("@poz", (object)z.Pozicija ?? DBNull.Value);
                p.AddWithValue("@rb", z.RbAdrese);
                p.AddWithValue("@opst", z.NazivOpstine);
            });
        }

        public void Delete(string mbr)
            => _db.Execute("DELETE FROM dbo.Zaposleni WHERE MaticniBrojZaposlenog=@mbr", p => p.AddWithValue("@mbr", mbr));

        private static Zaposleni Map(IDataRecord r) => new Zaposleni
        {
            MaticniBrojZaposlenog = DatabaseUtils.GetString(r, "MaticniBrojZaposlenog"),
            BrojLicneKarte = DatabaseUtils.GetString(r, "BrojLicneKarte"),
            Ime = DatabaseUtils.GetString(r, "Ime"),
            Prezime = DatabaseUtils.GetString(r, "Prezime"),
            StrucnaSprema = DatabaseUtils.GetString(r, "StrucnaSprema"),
            Pozicija = DatabaseUtils.GetString(r, "Pozicija"),
            RbAdrese = DatabaseUtils.GetInt(r, "RbAdrese"),
            NazivOpstine = DatabaseUtils.GetString(r, "NazivOpstine")
        };
    }
}
