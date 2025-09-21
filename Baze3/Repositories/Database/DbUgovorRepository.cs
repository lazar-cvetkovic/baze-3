using Baze3.Database;
using Baze3.Domain;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Baze3.Repositories.Database
{
    public sealed class DbUgovorRepository : IUgovorRepository
    {
        private readonly IDatabase _db;

        public DbUgovorRepository(IDatabase db) => _db = db;

        public IEnumerable<UgovorORadu> GetAll()
        {
            const string sql = "SELECT MaticniBrojZaposlenog,MaticniBrojPreduzeca,DatumZakljucivanja,Aktivan,Naziv FROM dbo.UgovorORadu";
            foreach (var r in _db.Query(sql)) 
                yield return Map(r);
        }

        public IEnumerable<UgovorORadu> Search(string query)
        {
            const string sql = @"SELECT MaticniBrojZaposlenog,MaticniBrojPreduzeca,DatumZakljucivanja,Aktivan,Naziv
                                 FROM dbo.UgovorORadu
                                 WHERE Naziv LIKE @q OR MaticniBrojZaposlenog LIKE @q OR MaticniBrojPreduzeca LIKE @q";

            foreach (var r in _db.Query(sql, p => p.AddWithValue("@q", DatabaseUtils.Like(query)))) 
                yield return Map(r);
        }

        public void Add(UgovorORadu u)
        {
            const string sql = "INSERT INTO dbo.UgovorORadu(MaticniBrojZaposlenog,MaticniBrojPreduzeca,DatumZakljucivanja,Aktivan,Naziv) VALUES(@mz,@mp,@dt,@ak,@nz)";
            _db.Execute(sql, p =>
            {
                p.AddWithValue("@mz", u.MaticniBrojZaposlenog);
                p.AddWithValue("@mp", u.MaticniBrojPreduzeca);
                p.AddWithValue("@dt", u.DatumZakljucivanja.Date);
                p.AddWithValue("@ak", u.Aktivan);
                p.AddWithValue("@nz", u.Naziv);
            });
        }

        public void Update(UgovorORadu u)
        {
            const string sql = "UPDATE dbo.UgovorORadu SET Aktivan=@ak,Naziv=@nz WHERE MaticniBrojZaposlenog=@mz AND MaticniBrojPreduzeca=@mp AND DatumZakljucivanja=@dt";
            _db.Execute(sql, p =>
            {
                p.AddWithValue("@mz", u.MaticniBrojZaposlenog);
                p.AddWithValue("@mp", u.MaticniBrojPreduzeca);
                p.AddWithValue("@dt", u.DatumZakljucivanja.Date);
                p.AddWithValue("@ak", u.Aktivan);
                p.AddWithValue("@nz", u.Naziv);
            });
        }

        public void Otvori(string mbrZap, string mbrPred, System.DateTime datum)
        {
            _db.Execute("dbo.spOtvoriUgovor", p =>
            {
            });

            using (var con = new SqlConnection(_db.ConnectionString))
            using (var cmd = new SqlCommand("dbo.spOtvoriUgovor", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaticniBrojZaposlenog", mbrZap);
                cmd.Parameters.AddWithValue("@MaticniBrojPreduzeca", mbrPred);
                cmd.Parameters.AddWithValue("@DatumZakljucivanja", datum.Date);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Zatvori(string mbrZap, string mbrPred, System.DateTime datum)
        {
            using (var con = new SqlConnection(_db.ConnectionString))
            using (var cmd = new SqlCommand("dbo.spZatvoriUgovor", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaticniBrojZaposlenog", mbrZap);
                cmd.Parameters.AddWithValue("@MaticniBrojPreduzeca", mbrPred);
                cmd.Parameters.AddWithValue("@DatumZakljucivanja", datum.Date);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        private static UgovorORadu Map(IDataRecord r) => new UgovorORadu
        {
            MaticniBrojZaposlenog = DatabaseUtils.GetString(r, "MaticniBrojZaposlenog"),
            MaticniBrojPreduzeca = DatabaseUtils.GetString(r, "MaticniBrojPreduzeca"),
            DatumZakljucivanja = DatabaseUtils.GetDate(r, "DatumZakljucivanja"),
            Aktivan = DatabaseUtils.GetString(r, "Aktivan"),
            Naziv = DatabaseUtils.GetString(r, "Naziv")
        };
    }
}