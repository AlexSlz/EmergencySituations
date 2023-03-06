﻿using EmergencySituations.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.OleDb;

#pragma warning disable CA1416 // Проверка совместимости платформы
namespace EmergencySituations.DataBase
{
    public static class MyDataBase
    {
        public static MyDBContext<User> Users = new MyDBContext<User>("Користувачі");

        private static OleDbConnection _conn = null;

        public static void Connect(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Persist Security Info=False;";
            _conn = new OleDbConnection(connString);
            TryConnectToDB(_conn);
            Users.Load();
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

        public static bool AddRow(string tableName, IModel obj)
        {
            var data = obj.ToDictionary();

            string q = $"INSERT INTO [{tableName}] ({String.Join(", ", data.Keys.ToArray())}) VALUES (@{String.Join(", @", data.Keys.ToArray())})";
            return RowAction(data, q);
        }

        public static bool UpdateRow(string tableName, IModel obj)
        {
            var data = obj.ToDictionary();

            var temp = new List<string>();
            foreach (var row in data)
            {
                temp.Add($"[{row.Key}] = @{row.Key}");
            }
            string q = $"UPDATE [{tableName}] SET {String.Join(", ", temp.ToArray())} WHERE [Код] = {obj.Код}";
            return RowAction(data, q);
        }

        private static bool RowAction(Dictionary<string, object> data, string q)
        {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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