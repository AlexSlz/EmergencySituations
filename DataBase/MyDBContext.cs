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

        public MyDBContext(object data, string tableName = "")
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
            if (data != null)
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
            return (this.Count > 0) ? int.Parse(this.MaxBy(x => x["Код"])["Код"].ToString()) : 0;
        }

        public MyDBContext Update(string tableName)
        {
            if (this[0].ContainsKey("Код"))
            {
                var temp = this.Select(i => i["Код"]);
                this.Clear();
                var sql = $"SELECT * FROM [{tableName}] WHERE [Код] in ({String.Join(", ", temp)})";
                Console.Write(sql);
                var data = MyDataBase.GetData(sql);
                if (data != null)
                    this.AddRange(data);
            }
            else
            {
                var n = new MyDBContext(tableName);
                var c = this.Count;
                this.Clear();
                this.AddRange(n.TakeLast(c));
            }
            GC.Collect();
            return this;
        }

        public MyDBContext FindAllByOld(MyDBContext obj)
        {
            //var result = new MyDBContext();
            return null;
/*            this.ForEach(i =>
            {
                obj.ForEach(o =>
                {
                    if (i["Код"] == o["Код"]) result.Add(i);
                });
            });
            return result;*/
        }

        ~MyDBContext()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
