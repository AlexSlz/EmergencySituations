using EmergencySituations.Model;

namespace EmergencySituations.Auth
{
    public class Token
    {
        private static List<TokenData> ActiveTokens = new List<TokenData>();

        public static string GenerateToken(string name)
        {
            var time = DateTime.Now.AddDays(1);
            var token = new TokenData(name, time);
            ActiveTokens.Add(token);
            return token.Key.ToString();
        }

        public static bool Validation(string key)
        {
            var deleteItems = ActiveTokens.FindAll(token => DateTime.Now > token.TimeEnd);
            ActiveTokens = ActiveTokens.Except(deleteItems).ToList();

            var find = ActiveTokens.Find(k => k.Key.ToString() == key);
            return find != null;
        }

        public static string GetOwner(string token)
        {
            return ActiveTokens.Find(t => t.Key.ToString() == token).Owner;
        }

    }
}
