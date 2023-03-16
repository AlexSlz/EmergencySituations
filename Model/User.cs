using EmergencySituations.DataBase;
using Newtonsoft.Json;

namespace EmergencySituations.Model
{
    public class User : IModel
    {
        [JsonProperty("Код")]
        public int Id { get; set; }
        [JsonProperty("Ім'я")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("Контактні дані")]
        public string ContactDetails { get; set; } = string.Empty;

        [JsonProperty("Логін")]
        public string Login { get; set; } = string.Empty;
        [JsonProperty("Пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
