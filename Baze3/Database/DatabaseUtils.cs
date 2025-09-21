using System;
using System.Data;

namespace Baze3.Database
{
    internal static class DatabaseUtils
    {
        public static string Like(string input) => "%" + (input ?? string.Empty).Trim() + "%";
        public static TimeSpan HoursToTime(decimal? hours) => TimeSpan.FromHours((double)(hours ?? 0m));
        public static decimal? TimeToHoursNull(TimeSpan ts) => (decimal)ts.TotalHours;

        public static string GetString(IDataRecord r, string name)
        {
            int i = r.GetOrdinal(name);
            return r.IsDBNull(i) ? string.Empty : Convert.ToString(r[i]);
        }

        public static int GetInt(IDataRecord r, string name)
        {
            int i = r.GetOrdinal(name);
            return r.IsDBNull(i) ? 0 : Convert.ToInt32(r[i]);
        }

        public static DateTime GetDate(IDataRecord r, string name)
        {
            int i = r.GetOrdinal(name);
            return r.IsDBNull(i) ? DateTime.MinValue : Convert.ToDateTime(r[i]);
        }

        public static decimal? GetDecimalNull(IDataRecord r, string name)
        {
            int i = r.GetOrdinal(name);
            return r.IsDBNull(i) ? (decimal?)null : Convert.ToDecimal(r[i]);
        }
    }
}
