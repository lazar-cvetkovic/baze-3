using Baze3.Database;
using Baze3.Domain;
using System.Collections.Generic;
using System.Data;

namespace Baze3.Repositories.Database
{
    public sealed class DbAdresaRepository : IAdresaRepository
    {
        private readonly IDatabase _db;
        public DbAdresaRepository(IDatabase db) => _db = db;

        public IEnumerable<Adresa> GetAll()
        {
            const string sql = "SELECT RbAdrese,BrojStana,Ulica,RbMesta FROM dbo.Adresa ORDER BY RbAdrese";
            foreach (var r in _db.Query(sql)) yield return Map(r);
        }

        public IEnumerable<Adresa> FindByUlica(string ulica)
        {
            const string sql = "SELECT RbAdrese,BrojStana,Ulica,RbMesta FROM dbo.Adresa WHERE Ulica LIKE @q ORDER BY Ulica";
            foreach (var r in _db.Query(sql, p => p.AddWithValue("@q", DatabaseUtils.Like(ulica)))) yield return Map(r);
        }

        public bool Exists(int rbAdrese)
            => _db.Scalar<int>("SELECT COUNT(1) FROM dbo.Adresa WHERE RbAdrese=@id", p => p.AddWithValue("@id", rbAdrese)) > 0;

        public Adresa Get(int rbAdrese)
        {
            foreach (var r in _db.Query("SELECT RbAdrese,BrojStana,Ulica,RbMesta FROM dbo.Adresa WHERE RbAdrese=@id",
                     p => p.AddWithValue("@id", rbAdrese))) return Map(r);
            return null;
        }

        public void Add(Adresa a)
        {
            const string sql = @"INSERT INTO dbo.Adresa(RbAdrese,BrojStana,Ulica,RbMesta)
                                 VALUES(@rb,(CASE WHEN @br='' THEN NULL ELSE @br END),@ul,@rm)";
            _db.Execute(sql, p =>
            {
                p.AddWithValue("@rb", a.RbAdrese);
                p.AddWithValue("@br", a.BrojStana ?? string.Empty);
                p.AddWithValue("@ul", a.Ulica);
                p.AddWithValue("@rm", a.RbMesta);
            });
        }

        public void Update(Adresa a)
        {
            const string sql = @"UPDATE dbo.Adresa SET BrojStana=(CASE WHEN @br='' THEN NULL ELSE @br END),
                                 Ulica=@ul, RbMesta=@rm WHERE RbAdrese=@rb";
            _db.Execute(sql, p =>
            {
                p.AddWithValue("@rb", a.RbAdrese);
                p.AddWithValue("@br", a.BrojStana ?? string.Empty);
                p.AddWithValue("@ul", a.Ulica);
                p.AddWithValue("@rm", a.RbMesta);
            });
        }

        private static Adresa Map(IDataRecord r) => new Adresa
        {
            RbAdrese = DatabaseUtils.GetInt(r, "RbAdrese"),
            BrojStana = DatabaseUtils.GetString(r, "BrojStana"),
            Ulica = DatabaseUtils.GetString(r, "Ulica"),
            RbMesta = DatabaseUtils.GetInt(r, "RbMesta")
        };
    }
}
