namespace EmergencySituations.Model
{
    public class TokenData
    {
        public string Key;
        public DateTime TimeEnd;
        public TokenData(string key, DateTime timeEnd)
        {
            Key = key;
            TimeEnd = timeEnd;
        }
    }
}
