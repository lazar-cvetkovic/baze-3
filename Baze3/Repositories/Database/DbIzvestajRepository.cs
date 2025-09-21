using Baze3.Database;
using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Data;

namespace Baze3.Repositories.Database
{
    public sealed class DbIzvestajRepository : IIzvestajRepository
    {
        private readonly IDatabase _db;

        public DbIzvestajRepository(IDatabase db) => _db = db;

        public IEnumerable<IzvestajZaposlenog> GetAll()
        {
            const string sql = "SELECT RbIzvestaja,MaticniBrojZaposlenog,UkupnoRadnoVremeNaIstrazivanjuIRazvoju,UkupnoRadnoVreme,Ime,Prezime,OpisAktivnosti,Napomena FROM dbo.v_IzvestajZaposlenog";
            foreach (var r in _db.Query(sql))
                yield return Map(r);
        }

        public IEnumerable<IzvestajZaposlenog> Search(string q)
        {
            const string sql = @"SELECT RbIzvestaja,MaticniBrojZaposlenog,UkupnoRadnoVremeNaIstrazivanjuIRazvoju,UkupnoRadnoVreme,Ime,Prezime,OpisAktivnosti,Napomena
                                 FROM dbo.v_IzvestajZaposlenog
                                 WHERE Ime LIKE @q OR Prezime LIKE @q OR MaticniBrojZaposlenog LIKE @q";

            foreach (var r in _db.Query(sql, p => p.AddWithValue("@q", DatabaseUtils.Like(q))))
                yield return Map(r);
        }

        public void Add(IzvestajZaposlenog iz)
        {
            _db.Transaction((con, tx) =>
            {
                using (var c1 = con.CreateCommand())
                {
                    c1.Transaction = tx;
                    c1.CommandText = @"INSERT INTO dbo.IzvestajZaposlenogNaIstrazivanjuIRazvoju
                                        (RbIzvestaja,UkupnoRadnoVremeNaIstrazivanjuIRazvoju,UkupnoRadnoVreme,MaticniBrojZaposlenog,Ime,Prezime)
                                        VALUES(@rb,@ir,@uk,@mz,@ime,@prez)";
                    c1.Parameters.AddWithValue("@rb", iz.RbIzvestaja);
                    c1.Parameters.AddWithValue("@ir", (object)DatabaseUtils.TimeToHoursNull(iz.UkupnoRadnoVremeNaIstrazivanjuIRazvoju) ?? DBNull.Value);
                    c1.Parameters.AddWithValue("@uk", (object)DatabaseUtils.TimeToHoursNull(iz.UkupnoRadnoVreme) ?? DBNull.Value);
                    c1.Parameters.AddWithValue("@mz", iz.MaticniBrojZaposlenog);
                    c1.Parameters.AddWithValue("@ime", iz.Ime);
                    c1.Parameters.AddWithValue("@prez", iz.Prezime);
                    c1.ExecuteNonQuery();
                }
                using (var c2 = con.CreateCommand())
                {
                    c2.Transaction = tx;
                    c2.CommandText = @"INSERT INTO dbo.IzvestajZaposlenog_Detalji(RbIzvestaja,OpisAktivnosti,Napomena) VALUES(@rb,@opis,@nap)";
                    c2.Parameters.AddWithValue("@rb", iz.RbIzvestaja);
                    c2.Parameters.AddWithValue("@opis", iz.OpisAktivnosti ?? string.Empty);
                    c2.Parameters.AddWithValue("@nap", (object)iz.Napomena ?? DBNull.Value);
                    c2.ExecuteNonQuery();
                }
            });
        }

        public void Update(IzvestajZaposlenog iz)
        {
            _db.Transaction((con, tx) =>
            {
                using (var c1 = con.CreateCommand())
                {
                    c1.Transaction = tx;
                    c1.CommandText = @"UPDATE dbo.IzvestajZaposlenogNaIstrazivanjuIRazvoju
                                        SET UkupnoRadnoVremeNaIstrazivanjuIRazvoju=@ir,UkupnoRadnoVreme=@uk,
                                            MaticniBrojZaposlenog=@mz,Ime=@ime,Prezime=@prez
                                        WHERE RbIzvestaja=@rb";
                    c1.Parameters.AddWithValue("@rb", iz.RbIzvestaja);
                    c1.Parameters.AddWithValue("@ir", (object)DatabaseUtils.TimeToHoursNull(iz.UkupnoRadnoVremeNaIstrazivanjuIRazvoju) ?? DBNull.Value);
                    c1.Parameters.AddWithValue("@uk", (object)DatabaseUtils.TimeToHoursNull(iz.UkupnoRadnoVreme) ?? DBNull.Value);
                    c1.Parameters.AddWithValue("@mz", iz.MaticniBrojZaposlenog);
                    c1.Parameters.AddWithValue("@ime", iz.Ime);
                    c1.Parameters.AddWithValue("@prez", iz.Prezime);
                    c1.ExecuteNonQuery();
                }
                using (var c2 = con.CreateCommand())
                {
                    c2.Transaction = tx;
                    c2.CommandText = @"IF EXISTS(SELECT 1 FROM dbo.IzvestajZaposlenog_Detalji WHERE RbIzvestaja=@rb)
                                        UPDATE dbo.IzvestajZaposlenog_Detalji SET OpisAktivnosti=@opis,Napomena=@nap WHERE RbIzvestaja=@rb
                                        ELSE INSERT INTO dbo.IzvestajZaposlenog_Detalji(RbIzvestaja,OpisAktivnosti,Napomena) VALUES(@rb,@opis,@nap)";
                    c2.Parameters.AddWithValue("@rb", iz.RbIzvestaja);
                    c2.Parameters.AddWithValue("@opis", iz.OpisAktivnosti ?? string.Empty);
                    c2.Parameters.AddWithValue("@nap", (object)iz.Napomena ?? DBNull.Value);
                    c2.ExecuteNonQuery();
                }
            });
        }

        private static IzvestajZaposlenog Map(IDataRecord r) => new IzvestajZaposlenog
        {
            RbIzvestaja = DatabaseUtils.GetInt(r, "RbIzvestaja"),
            MaticniBrojZaposlenog = DatabaseUtils.GetString(r, "MaticniBrojZaposlenog"),
            UkupnoRadnoVremeNaIstrazivanjuIRazvoju = DatabaseUtils.HoursToTime(DatabaseUtils.GetDecimalNull(r, "UkupnoRadnoVremeNaIstrazivanjuIRazvoju")),
            UkupnoRadnoVreme = DatabaseUtils.HoursToTime(DatabaseUtils.GetDecimalNull(r, "UkupnoRadnoVreme")),
            Ime = DatabaseUtils.GetString(r, "Ime"),
            Prezime = DatabaseUtils.GetString(r, "Prezime"),
            OpisAktivnosti = DatabaseUtils.GetString(r, "OpisAktivnosti"),
            Napomena = DatabaseUtils.GetString(r, "Napomena")
        };
    }
}