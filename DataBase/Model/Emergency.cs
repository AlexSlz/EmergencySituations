namespace EmergencySituations.DataBase.Model
{
    public class Emergency : IDBTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        [RelationKey(RelationType.ManyToOne)]
        public EmergencyLevel Level { get; set; }
        [RelationKey(RelationType.ManyToOne)]
        public EmergencyType Type { get; set; }
        public string Image { get; set; }

        [RelationKey(RelationType.OneToOne)]
        public Losses Losses { get; set; }

        [RelationKey("EmergencyId", RelationType.OneToMany)]
        public List<Positions> Positions { get; set; }
        public int AddedBy { get; set; }

        public static string Sql => @"
            CREATE TABLE IF NOT EXISTS ""Emergency"" (
	            ""Id""	INTEGER NOT NULL,
	            ""Name""	TEXT,
	            ""Description""	TEXT,
	            ""DateAndTime""	DATETIME,
	            ""Level""	INTEGER NOT NULL,
	            ""Type""	INTEGER NOT NULL,
	            ""Image""	TEXT,
	            ""Losses""	INTEGER NOT NULL,
	            ""AddedBy""	INTEGER,

	            FOREIGN KEY(""Level"") REFERENCES ""EmergencyLevel""(""Id""),
                FOREIGN KEY(""Type"") REFERENCES ""EmergencyType""(""Id""),
                FOREIGN KEY(""Losses"") REFERENCES ""Losses""(""Id""),
	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );
        ";
        //FOREIGN KEY(""AddedBy"") REFERENCES ""Users""(""Id""),
    }
}
