using EmergencySituations.Auth;
using EmergencySituations.DataBase.Model;
using System;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Reflection.PortableExecutable;
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
            if(Count<Users>() <= 0)
                Insert(new Users() { Name = "Alex", Login = "Admin", Password = "123" });

            Console.WriteLine(Token.GenerateToken("Alex"));

            ExecuteNonQuery(EmergencyLevel.Sql);
            if (Count<EmergencyLevel>() <= 0)
            {
                Insert(new EmergencyLevel() { Name = "Державний" });
                Insert(new EmergencyLevel() { Name = "Регіональний" });
                Insert(new EmergencyLevel() { Name = "Місцевий" });
                Insert(new EmergencyLevel() { Name = "Об'єктовий" });
            }

            ExecuteNonQuery(EmergencyType.Sql);
            if (Count<EmergencyType>() <= 0)
            {
                Insert(new EmergencyType() { Name = "Природного" });
                Insert(new EmergencyType() { Name = "Техногенного" });
                Insert(new EmergencyType() { Name = "Екологічного" });
                Insert(new EmergencyType() { Name = "Соціального" });
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
            using (var sql = new DataBaseConnection())
            {
                return sql.CreateCommand(q).Read().ToClass<T>();
            }
        }

        public static dynamic SelectTable(Type T, string q = "")
        {
            var method = typeof(MyDataBase).GetMethod(nameof(MyDataBase.Select));
            var genericMethod = method.MakeGenericMethod(T)!;
            return genericMethod.Invoke(null, new string[] { q });
        }

        public static int Count<T>() where T : IDBTable
        {
            string q = $"SELECT COUNT(*) FROM {typeof(T).Name}";
            using (var sql = new DataBaseConnection())
            {
                return Convert.ToInt32(sql.CreateCommand(q).ExecuteScalar());
            }
        } 

        public static IEnumerable<string> GetTableNameList()
        {
            string q = "SELECT name FROM sqlite_master WHERE type = \"table\"";
            using (var sql = new DataBaseConnection())
            {
                var a = sql.CreateCommand(q).Read().Select(i => i.First()).Select(i => i.Value.ToString()).Where(i => i != "sqlite_sequence");
                return a;
            }
        }

        public static Dictionary<string, string> GetTableKeys<T>()
        {
            return typeof(T).GetProperties().Where(i => i.Name != "Sql").ToDictionary(t => t.Name, t => t.PropertyType.Name);
        }

    }

    public static class MyDBExtension
    {
        public static List<Dictionary<string, object>> Read(this SQLiteCommand cmd)
        {
            using (var reader = cmd.ExecuteReader())
            {
                List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
                while (reader.Read())
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.GetFieldType(i).Name == "DateTime" ? 
                            reader.GetString(i) 
                            : reader[i];
                        data.Add(reader.GetName(i), value);
                    }

                    result.Add(data);
                }

                return result;
            }
        }

        public static (string tableName, Dictionary<string, string> data) GetClassData(IDBTable table)
        {
            var type = table.GetType();

            var data = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(i => $"{i.GetValue(table, null)}" != "" && i.Name != "Id")
                .ToDictionary(i => i.Name, i => $"{i.GetValue(table, null)}");

            return (tableName: type.Name, data: data);
        }

        public static List<T> ToClass<T>(this List<Dictionary<string, object>> data) where T : IDBTable
        {
            var table = MyDataBase.GetTableNameList();
            var properties = typeof(T).GetProperties();
            return data.Select(row =>
            {
                var cls = Activator.CreateInstance<T>();
                foreach (var property in properties)
                {
                    Type type = null;
                    var pName = property.PropertyType.Name;
                    if (pName == typeof(List<>).Name)
                        foreach (var item in table)
                        {
                            if (item == property.Name)
                                type = property.PropertyType.GetGenericArguments()[0];
                        }
                    if (row.ContainsKey(property.Name))
                    {
                        var value = Convert.ChangeType(row[property.Name], property.PropertyType);
                        property.SetValue(cls, value);
                    }
                    if(type != null)
                    {
                        var temp = (RelationKey)property.GetCustomAttribute(typeof(RelationKey));
                        if(temp != null)
                            property.SetValue(cls, MyDataBase.SelectTable(type, $"SELECT * FROM {type.Name} WHERE [{temp.KeyName}] = {((T)cls).Id}"));
                    }
                }
                return cls;
            }).ToList();
        }
    }
}
