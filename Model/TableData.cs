using Newtonsoft.Json;

namespace EmergencySituations.Model
{
    public class TableData
    {
        public string Name;
        public string Type;
        public string Value;
        public TableData(string name, string type, string value = "")        {
            Name = name;
            Type = type;
            Value = value;
        }

    }
}
