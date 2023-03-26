using EmergencySituations.Model;

using System.Text;


namespace EmergencySituations.Auth
{
    public static class Token
    {
        private static List<TokenData> ActiveTokens = new List<TokenData>();

        public static string GenerateToken(string name)
        {
            var key = Crypt(name);
            if (Validation(key))
            {
                return key;
            }
            var time = DateTime.Now.AddDays(1);

            ActiveTokens.Add(new TokenData(key, time));
            return key;
        }

        public static bool Validation(string key)
        {
            var deleteItems = ActiveTokens.FindAll(token => DateTime.Now > token.TimeEnd);
            ActiveTokens = ActiveTokens.Except(deleteItems).ToList();

            var find = ActiveTokens.Find(k => k.Key == key);
            return find != null;
        }

        private static string Crypt(string key)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
        }
        public static string Decrypt(string key)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(key));
        }

    }
}
