using Newtonsoft.Json;

namespace EmergencySituations.DataBase
{
    public class MyDBContext<T> : List<T> where T : IModel
    {
        private string _tableName;

        public MyDBContext(string tableName)
        {
            _tableName = tableName;
        }

        public static implicit operator string(MyDBContext<T> t) => JsonConvert.SerializeObject(t);

        public void Load()
        {
            string json = MyDataBase.GetData($"SELECT * FROM [{_tableName}]");
            this.Clear();
            this.AddRange(JsonConvert.DeserializeObject<List<T>>(json));
        }

        public void UpdateTable()
        {

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
