namespace EmergencySituations.DataBase
{
    public enum RelationType
    {
        OneToOne,
        OneToMany,
        ManyToOne,
    }
    public class RelationKey : Attribute
    {
        public string KeyName { get; }
        public RelationType RelationType { get; }
        public RelationKey(string keyName, RelationType relationType)
        {
            KeyName = keyName;
            RelationType = relationType;
        }
        public RelationKey(RelationType relationType) : this("", relationType) { }
    }
}
