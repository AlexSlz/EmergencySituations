using EmergencySituations.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.OleDb;

#pragma warning disable CA1416 // Проверка совместимости платформы
namespace EmergencySituations.DataBase
{
    public static class MyDataBase
    {
        private static OleDbConnection _conn = null;

        public static void Connect(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Persist Security Info=False;";
            _conn = new OleDbConnection(connString);
            TryConnectToDB(_conn);
        }

        public static List<Dictionary<string, dynamic>> GetData(string q)
        {
            if (string.IsNullOrEmpty(q))
                return null;
            try
            {
                using (DataTable table = new DataTable())
                using (OleDbCommand cmd = new OleDbCommand(q, _conn))
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                    return table.AsEnumerable().Select(
                        row => table.Columns.Cast<DataColumn>().ToDictionary(
                            column => column.ColumnName,
                            column => (string.IsNullOrEmpty(row[column].ToString())) ? "" : row[column])).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] {q}\n{ex.Message}");
                return null;
            }
        }

        public static bool AddRow(string tableName, Dictionary<string, object> data)
        {
            var temp = GetValue(data, tableName);
            string q = $"INSERT INTO [{tableName}] ([{String.Join("], [", data.Keys.ToArray())}]) VALUES ({String.Join(", ", temp.ToArray())})";
            return ExecuteQuery(q);
        }

        public static bool UpdateRow(string tableName, Dictionary<string, object> data)
        {
            var temp = GetValue(data, tableName, true);
            string q = $"UPDATE [{tableName}] SET {String.Join(", ", temp.ToArray())} WHERE [Код] = {data["Код"]}";
            return ExecuteQuery(q);
        }

        private static List<string> GetValue(Dictionary<string, object> data, string tableName, bool update = false)
        {
            var temp = new List<string>();
            var columnName = GetKeyTypeTable(tableName);
            foreach (var row in data)
            {
                string value = $"'{row.Value}'";
                if (columnName[row.Key] == "Int32")
                {
                    value = (row.Value.ToString());
                }
                if (row.Key.Contains("Дата"))
                {
                    value = value.Replace('T', ' ');
                }
                temp.Add($"{(update ? $"[{row.Key}] =" : "")}{value}");
            }
            return temp;
        }

        private static bool ExecuteQuery(string q)
        {
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(q, _conn))
                {
                    cmd.ExecuteNonQuery();
                }
                //Console.WriteLine($"[+] {q}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] {q}\n{ex.Message}");
                return false;
            }
        }

        public static string ExecuteQueryWithValue(string q)
        {
            using (OleDbCommand cmd = new OleDbCommand(q, _conn))
            {
                return cmd.ExecuteScalar().ToString();
            }
        }

        public static bool DeleteRowById(string tableName, int id)
        {
            string q = $"DELETE FROM [{tableName}] WHERE [Код] = {id}";
            return ExecuteQuery(q);
        }

        public static Dictionary<string, string> GetKeyTypeTable(string tableName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();


            string q = $"SELECT * FROM [{tableName}]";


            using(OleDbCommand cmd = new OleDbCommand(q, _conn))
            using(OleDbDataReader rdr = cmd.ExecuteReader())
            {
                rdr.Read();
                for (int i = 0; i < rdr.VisibleFieldCount; i++)
                {
                    System.Type type = rdr.GetFieldType(i);

                    result.Add(rdr.GetName(i), type.Name);
                }
            }

            return result;
        }

        public static List<string> GetTableNameList()
        {
            List<string> list = new List<string>();
            DataTable dt = _conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow dr in dt.Rows)
            {
                if (!dr["TABLE_NAME"].ToString().Contains("~"))
                    list.Add(dr["TABLE_NAME"].ToString());
            }
            return list;
        }

        private static void TryConnectToDB(OleDbConnection conn)
        {
            try
            {
                conn.Open();
                Console.WriteLine("\n[+] DB Connected\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n[!] DB NOT Connected\n");
                Console.WriteLine(ex.Message);
                Console.WriteLine("\n[?]ReConnect?\n");
                if (string.IsNullOrEmpty(Console.ReadLine()))
                {
                    TryConnectToDB(conn);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
        private static void Disconnect(OleDbConnection conn)
        {
            conn.Close();
            GC.SuppressFinalize(conn);
            GC.Collect();
        }
    }
}
#pragma warning restore CA1416 // Проверка совместимости платформы