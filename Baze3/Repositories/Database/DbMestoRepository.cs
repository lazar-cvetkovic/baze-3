using Baze3.Database;
using Baze3.Domain;
using System.Collections.Generic;
using System.Data;

namespace Baze3.Repositories.Database
{
    public sealed class DbMestoRepository : IMestoRepository
    {
        private readonly IDatabase _db;
        public DbMestoRepository(IDatabase db) => _db = db;

        public IEnumerable<Mesto> GetByOpstina(int rbOpstine)
        {
            const string sql = "SELECT RbMesta,NazivMesta,RbOpstine FROM dbo.Mesto WHERE RbOpstine=@o ORDER BY NazivMesta";
            foreach (var r in _db.Query(sql, p => p.AddWithValue("@o", rbOpstine))) yield return Map(r);
        }

        public bool Exists(int rbMesta)
            => _db.Scalar<int>("SELECT COUNT(1) FROM dbo.Mesto WHERE RbMesta=@id", p => p.AddWithValue("@id", rbMesta)) > 0;

        public void Add(Mesto m)
        {
            _db.Execute("INSERT INTO dbo.Mesto(RbMesta,NazivMesta,RbOpstine) VALUES(@rb,@nz,@o)",
                p => {
                    p.AddWithValue("@rb", m.RbMesta);
                    p.AddWithValue("@nz", m.NazivMesta);
                    p.AddWithValue("@o", m.RbOpstine);
                });
        }

        private static Mesto Map(IDataRecord r) => new Mesto
        {
            RbMesta = DatabaseUtils.GetInt(r, "RbMesta"),
            NazivMesta = DatabaseUtils.GetString(r, "NazivMesta"),
            RbOpstine = DatabaseUtils.GetInt(r, "RbOpstine")
        };
    }
}
