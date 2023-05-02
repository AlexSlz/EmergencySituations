using EmergencySituations.DataBase.Model;
namespace EmergencySituations.Other.Model
{
    public class TokenData
    {
        public string Owner;
        public Guid Key;
        public DateTime TimeEnd;
        public TokenData(string owner, DateTime timeEnd)
        {
            Owner = owner;
            Key = Guid.NewGuid();
            TimeEnd = timeEnd;
        }
    }
}
