using Baze3.Database;
using Baze3.Domain;
using System.Collections.Generic;
using System.Data;

namespace Baze3.Repositories.Database
{
    public sealed class DbOpstinaRepository : IOpstinaRepository
    {
        private readonly IDatabase _db;
        public DbOpstinaRepository(IDatabase db) => _db = db;

        public IEnumerable<Opstina> GetAll()
        {
            const string sql = "SELECT RbOpstine, NazivOpstine FROM dbo.Opstina ORDER BY NazivOpstine";
            foreach (var r in _db.Query(sql)) yield return Map(r);
        }

        public bool Exists(int rb)
            => _db.Scalar<int>("SELECT COUNT(1) FROM dbo.Opstina WHERE RbOpstine=@id", p => p.AddWithValue("@id", rb)) > 0;

        public void Add(Opstina o)
        {
            _db.Execute("INSERT INTO dbo.Opstina(RbOpstine,NazivOpstine) VALUES(@rb,@nz)",
                p => { p.AddWithValue("@rb", o.RbOpstine); p.AddWithValue("@nz", o.NazivOpstine); });
        }

        private static Opstina Map(IDataRecord r) => new Opstina
        {
            RbOpstine = DatabaseUtils.GetInt(r, "RbOpstine"),
            NazivOpstine = DatabaseUtils.GetString(r, "NazivOpstine")
        };
    }
}
