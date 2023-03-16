using EmergencySituations.DataBase;
using Newtonsoft.Json;

namespace EmergencySituations.Model
{
    public class Emergency : IModel
    {
        [JsonProperty("Код")]
        public int Id { get; set; }
        [JsonProperty("Назва")]
        public string Name { get; set; }
        [JsonProperty("Опис")]
        public string Description { get; set; }
        [JsonProperty("Дата та час")]
        public string Date { get; set; }
        [JsonProperty("Рівень")]
        public string Level { get; set; }
        [JsonProperty("Тип")]
        public string Type { get; set; }
        [JsonProperty("Зображення")]
        public string Image { get; set; }
        [JsonProperty("Збитки")]
        public string Damages { get; set; }        
        [JsonProperty("Додав")]
        public string Сreator { get; set; }
    }
}
