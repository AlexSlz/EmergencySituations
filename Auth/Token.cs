using EmergencySituations.DataBase.Model;
using EmergencySituations.Other.Model;

namespace EmergencySituations.Auth
{
    public class Token
    {
        private static List<TokenData> ActiveTokens = new List<TokenData>();

        public static TokenData GenerateToken(string user)
        {
            var time = DateTime.Now.AddDays(1);
            var token = new TokenData(user, time);
            ActiveTokens.Add(token);
            return token;
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
