using EmergencySituations.Auth;
using EmergencySituations.DataBase.Model;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

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
            if (Count<Users>() <= 0)
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
            ExecuteNonQuery(Losses.Sql);
        }

        private static string ExecuteNonQuery(string q, Dictionary<string, object> args = null)
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
            if (obj.Id <= 0)
            {
                return Insert(obj);
            }

            var data = MyDBExtension.GetClassData(obj);
            var temp = data.data.Select(x => $"[{x.Key}] = @{x.Key}").ToArray();
            string q = $@"UPDATE [{data.tableName}] SET {String.Join(", ", temp)} WHERE Id = {obj.Id}";
            return ExecuteNonQuery(q, data.data);
        }

        public static string Delete(IDBTable obj)
        {
            string q = $"DELETE FROM [{obj.GetType().Name}] WHERE [Id] = {obj.Id}";
            return ExecuteNonQuery(q);
        }

        public static string Delete<T>(int id) where T : IDBTable
        {
            var obj = Activator.CreateInstance<T>();
            obj.Id = id;
            return Delete(obj);
        }

        public static List<T> Select<T>(string q = "") where T : IDBTable
        {
            if (string.IsNullOrEmpty(q))
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

        public static IEnumerable<string> GetImageList(string tableName)
        {
            string q = $"SELECT Image FROM [{tableName}]";
            using (var sql = new DataBaseConnection())
            {
                var a = new List<string>();
                sql.CreateCommand(q).Read().ForEach(l =>
                {
                    a.Add(l.Select(i => $"{i.Value}").First());
                });
                return a;
            }
        }

        public static int GetLastId(string tableName)
        {
            string q = $"SELECT MAX(id) FROM  [{tableName}]";
            using (var sql = new DataBaseConnection())
            {
                return Convert.ToInt32(sql.CreateCommand(q).ExecuteScalar());
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

        public static (string tableName, Dictionary<string, object> data) GetClassData(IDBTable table)
        {
            var tables = MyDataBase.GetTableNameList();
            var type = table.GetType();

            var data = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(i => i.Name != "Id"
                                && (RelationKey)i.GetCustomAttribute(typeof(RelationKey)) == null)
                .ToDictionary(i => i.Name, i =>
                {
                    var temp = tables.ToList().Find(t => t == i.Name);
                    if (temp != null)
                    {

                        var l = (IDBTable)i.GetValue(table, null);
                        if (l.Id == 0)
                        {
                            MyDataBase.Insert(l);
                            l.Id = MyDataBase.GetLastId(i.PropertyType.Name);
                        }
                        MyDataBase.Update(l);
                        return l.Id;
                    }
                    return i.GetValue(table, null);
                });

            return (tableName: type.Name, data);
        }

        public static List<T> ToClass<T>(this List<Dictionary<string, object>> data) where T : IDBTable
        {
            var table = MyDataBase.GetTableNameList();
            var properties = typeof(T).GetProperties();
            return data.Select(row =>
            {
                var cls = Activator.CreateInstance<T>();
                bool skip = false;
                foreach (var property in properties)
                {
                    var pName = property.PropertyType.Name;
                    foreach (var item in table)
                    {
                        if (item == property.Name)
                        {
                            if (pName == typeof(List<>).Name)
                            {
                                var type = property.PropertyType.GetGenericArguments()[0];
                                var temp = (RelationKey)property.GetCustomAttribute(typeof(RelationKey));
                                if (temp != null)
                                {
                                    property.SetValue(cls, MyDataBase
                                        .SelectTable(type, $"SELECT * FROM {type.Name} WHERE [{temp.KeyName}] = {((T)cls).Id}"));
                                }
                                skip = true;
                            }
                            else
                            {
                                var type = property.PropertyType;
                                var temp = row[type.Name];
                                if (temp != System.DBNull.Value)
                                    property.SetValue(cls, MyDataBase.SelectTable(type, $"SELECT * FROM {type.Name} WHERE [Id] = {temp}")[0]);
                                skip = true;
                            }
                        }
                    }

                    if (row.ContainsKey(property.Name) && !skip)
                    {
                        var value = Convert.ChangeType(row[property.Name], property.PropertyType);
                        property.SetValue(cls, value);
                    }
                }
                return cls;
            }).ToList();
        }
    }
}
