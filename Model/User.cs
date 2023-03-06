using EmergencySituations.DataBase;

namespace EmergencySituations.Model
{
    public class User : IModel
    {
        public int Код { get; set; }
        public string Логін { get; set; } = string.Empty;
        public string Пароль { get; set; } = string.Empty;
    }
}
