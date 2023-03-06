using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public static ContentResult GetData(this ControllerBase controller, string q)
        {
            var result = GetData(q);
            return controller.Content(result, "application/json");
        }

        public static string GetData(string q)
        {
            if (string.IsNullOrEmpty(q))
                return null;
            DataTable table = new DataTable();
            OleDbCommand cmd = new OleDbCommand(q, _conn);
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                table.Load(reader);
            }
            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }

        public static bool AddRow(string tableName, object json)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

            string q = $"INSERT INTO [{tableName}] ({String.Join(", ", data.Keys.ToArray())}) VALUES (@{String.Join(", @", data.Keys.ToArray())})";

            try
            {
                using (OleDbCommand cmd = new OleDbCommand(q, _conn))
                {
                    foreach (var item in data)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteRow(string tableName, int id)
        {
            string q = $"DELETE FROM [{tableName}] WHERE [Код] = {id}";
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(q, _conn))
                {
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void TryConnectToDB(OleDbConnection conn)
        {
            try
            {
                conn.Open();
                Console.WriteLine("--- DB Connected [ :) ]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- DB NOT Connected [ :( ]");
                Console.WriteLine(ex.Message);
                Console.WriteLine("--- ReConnect?");
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
    }
}
#pragma warning restore CA1416 // Проверка совместимости платформы