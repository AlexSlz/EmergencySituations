using Newtonsoft.Json;

namespace EmergencySituations.DataBase
{
    public class MyDBContext : List<Dictionary<string, object>>
    {
        private string _tableName;
        public MyDBContext(string tableName)
        {
            _tableName = tableName;
            Load();
        }

        public MyDBContext(object data)
        {
            try
            {
                var i = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.ToString());
                this.Add(i);
            }
            catch
            {
                var i = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data.ToString());
                this.AddRange(i);
            }
        }

        private void Load()
        {
            this.Clear();
            GC.Collect();
            var data = MyDataBase.GetData($"SELECT * FROM [{_tableName}]");
            if(data != null)
            this.AddRange(data);
        }

        public Dictionary<string, object> FindById(object data, string key = "Код")
        {
            return this.FirstOrDefault(i => i[key].ToString() == data.ToString());
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public int GetMaxId()
        {
            return (this.Count > 0) ?  int.Parse(this.MaxBy(x => x["Код"])["Код"].ToString()) : 0;
        }

        ~MyDBContext()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
