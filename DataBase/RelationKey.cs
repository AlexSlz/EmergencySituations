namespace EmergencySituations.DataBase
{
    public class RelationKey : Attribute
    {
        public string KeyName { get; }
        public RelationKey(string keyName)
        {
            KeyName = keyName;
        }
    }
}
