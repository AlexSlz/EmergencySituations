using EmergencySituations.Model;
using Newtonsoft.Json;

namespace EmergencySituations.DataBase
{
    public class MyDBContext<T> : List<T> where T : IModel
    {
        private string _tableName;
        public string TableName => _tableName;

        public MyDBContext(string tableName)
        {
            _tableName = tableName;
        }

        public static implicit operator string(MyDBContext<T> t) => JsonConvert.SerializeObject(t);

        public void Load()
        {
            string json = MyDataBase.GetData($"SELECT * FROM [{_tableName}]");
            this.Clear();
            GC.Collect();
            this.AddRange(JsonConvert.DeserializeObject<List<T>>(json));
        }

        public new virtual void Add(T element)
        {
            MyDataBase.AddRow(_tableName, element);
            Load();
        }

        public new virtual void Remove(T element)
        {
            MyDataBase.DeleteRow(_tableName, element.Id);
            Load();
        }

        public int GetIndex(T element)
        {
            var find = this.Find(i => i.Id == element.Id);
            MyDataBase.UpdateRow(_tableName, element);
            return this.IndexOf(find);
        }
    }
    public static class MyDBContextStatic
    {
        public static Dictionary<string, object> ToDictionary(this IModel model)
        {
            var serializedModel = JsonConvert.SerializeObject(model);
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedModel);
            foreach (var pair in result)
            {
                if (pair.Key == "Код")
                {
                    result.Remove(pair.Key);
                    break;
                }
            }
            return result;
        }

        public static string ToJson(this IModel model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
