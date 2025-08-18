using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace NBA_App.Data
{
    public static class SQLRead
    {
        public static bool GetSafeBoolean(SqlDataReader reader, string ColumnName)
        {
            int ordinal = reader.GetOrdinal(ColumnName);
            return !reader.IsDBNull(ordinal) && reader.GetBoolean(ordinal);
        }
        public static double GetSafeDouble(SqlDataReader reader, string ColumnName)
        {
            int ordinal = reader.GetOrdinal(ColumnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetDouble(ordinal);
        }
        public static int GetSafeInt(SqlDataReader reader, string ColumnName)
        {
            int ordinal = reader.GetOrdinal(ColumnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
        }
        public static DateTime GetSafeDate(SqlDataReader reader, string ColumnName)
        {
            int ordinal = reader.GetOrdinal(ColumnName);
            return reader.IsDBNull(ordinal) ? new DateTime() : reader.GetDateTime(ordinal);
        }
        public static string GetSafeString(SqlDataReader reader, string ColumnName)
        {
            int ordinal = reader.GetOrdinal(ColumnName);
            return reader.IsDBNull(ordinal) ? "" : reader.GetString(ordinal);
        }
        public static byte GetSafeTinyInt(SqlDataReader reader, string ColumnName)
        {
            int ordinal = reader.GetOrdinal(ColumnName);
            return reader.IsDBNull(ordinal) ? (byte)0 : reader.GetByte(ordinal);
        }
        public static short GetSafeShort(SqlDataReader reader, string ColumnName)
        {
            int ordinal = reader.GetOrdinal(ColumnName);
            return reader.IsDBNull(ordinal) ? (short)0 : reader.GetInt16(ordinal);
        }

    }
}
