using EmergencySituations.DataBase;
using Newtonsoft.Json;

namespace EmergencySituations.Model
{
    public class Position : IModel
    {
        [JsonProperty("Код")]
        public int Id { get; set; }
        [JsonProperty("Розташування")]
        public string Country { get; set; }
        [JsonProperty("Код нс")]
        public int EmergencyId { get; set; }
        public double X { get; set; } 
        public double Y { get; set; }
    }
}
