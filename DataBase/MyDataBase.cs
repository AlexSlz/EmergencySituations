using EmergencySituations.DataBase.Model;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Xml;

namespace EmergencySituations.DataBase
{
    public static class MyDataBase
    {
        private static string sqlConfig;
        public static string SqlConfig => sqlConfig;
        public static void Setup(string databasePath)
        {
            sqlConfig = databasePath + ";foreign keys=true";
            ExecuteNonQuery(Users.Sql);

            ExecuteNonQuery(EmergencyLevel.Sql);
            if (MyDataBase.Count<EmergencyLevel>() <= 0)
            {
                MyDataBase.Insert(new EmergencyLevel() { Name = "Державний" });
                MyDataBase.Insert(new EmergencyLevel() { Name = "Регіональний" });
                MyDataBase.Insert(new EmergencyLevel() { Name = "Місцевий" });
                MyDataBase.Insert(new EmergencyLevel() { Name = "Об'єктовий" });
            }
            ExecuteNonQuery(EmergencyType.Sql);
            if (MyDataBase.Count<EmergencyType>() <= 0)
            {
                MyDataBase.Insert(new EmergencyType() { Name = "Природного" });
                MyDataBase.Insert(new EmergencyType() { Name = "Техногенного" });
                MyDataBase.Insert(new EmergencyType() { Name = "Екологічного" });
                MyDataBase.Insert(new EmergencyType() { Name = "Соціального" });
            }
            ExecuteNonQuery(Positions.Sql);
            ExecuteNonQuery(Emergency.Sql);
        }

        private static string ExecuteNonQuery(string q, Dictionary<string, string> args = null)
        {
            try
            {
                using (DataBaseConnection sql = new DataBaseConnection())
                {
                    var a = sql.CreateCommand(q, args).ExecuteNonQuery();
                    return $"Ok, {a}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{q} \n {ex.Message}");
                return ex.Message;
            }
        }

        public static string Insert(IDBTable obj)
        {
            var data = MyDBExtension.GetClassData(obj);
            if (data.data.Count <= 0) return "Zero";
            string q = $"INSERT INTO [{data.tableName}] ([{String.Join("], [", data.data.Keys.ToArray())}]) VALUES ({String.Join(", ", data.data.Values.Select(i => "?").ToArray())})";
            return ExecuteNonQuery(q, data.data);
        }

        public static string Update(IDBTable obj)
        {
            var data = MyDBExtension.GetClassData(obj);
            var temp = data.data.Select(x => $"[{x.Key}] = @{x.Key}").ToArray();
            string q = $@"UPDATE [{data.tableName}] SET {String.Join(", ", temp)} WHERE Id = {obj.Id}";
            Console.WriteLine(q);
            return ExecuteNonQuery(q, data.data);
        }

        public static string Delete(IDBTable obj)
        {
            string q = $"DELETE FROM [{obj.GetType().Name}] WHERE [Id] = {obj.Id}";
            return ExecuteNonQuery(q);
        }

        public static string Delete<T>(int id) where T : IDBTable
        {
            string q = $"DELETE FROM [{typeof(T).Name}] WHERE [Id] = {id}";
            return ExecuteNonQuery(q);
        }

        public static List<T> Select<T>(string q = "") where T : IDBTable
        {
            if(string.IsNullOrEmpty(q))
            q = $"SELECT * FROM {typeof(T).Name}";
            DataTable table = new DataTable();
            using (var sql = new DataBaseConnection())
            {
                using (var reader = sql.CreateCommand(q).ExecuteReader())
                {
                    table.Load(reader);
                }
            }
            return table.TableToClassList<T>();
        }

        public static int Count<T>() where T : IDBTable
        {
            string q = $"SELECT COUNT(*) FROM {typeof(T).Name}";
            using (var sql = new DataBaseConnection())
            {
                return Convert.ToInt32(sql.CreateCommand(q).ExecuteScalar());
            }
        } 

    }

    public static class MyDBExtension
    {
        public static (string tableName, Dictionary<string, string> data) GetClassData(IDBTable table)
        {
            var type = table.GetType();

            var data = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(i => $"{i.GetValue(table, null)}" != "" && i.Name != "Id")
                .ToDictionary(i => i.Name, i => $"{i.GetValue(table, null)}");

            return (tableName: type.Name, data: data);
        }
        public static List<T> TableToClassList<T>(this DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();

            
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        object value = row[pro.Name];
                        object safeValue = value == null || DBNull.Value.Equals(value)
                            ? null
                            : Convert.ChangeType(value, pro.PropertyType);
                        pro.SetValue(objT, safeValue);
                    }
                }
                return objT;
            }).ToList();
        }
    }
}
