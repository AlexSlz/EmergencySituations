namespace EmergencySituations.DataBase.Model
{
    public class Emergency : IDBTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public int Costs { get; set; }
        [RelationKey("EmergencyId")]
        public List<Positions> Positions { get; set; }
        public int AddedBy { get; set; }

        public static string Sql => @"
            CREATE TABLE IF NOT EXISTS ""Emergency"" (
	            ""Id""	INTEGER NOT NULL,
	            ""Name""	TEXT,
	            ""Description""	TEXT,
	            ""DateAndTime""	DATETIME,
	            ""Level""	TEXT NOT NULL,
	            ""Type""	TEXT NOT NULL,
	            ""Image""	TEXT,
	            ""Costs""	INTEGER,
	            ""AddedBy""	INTEGER,

	            FOREIGN KEY(""Level"") REFERENCES ""EmergencyLevel""(""Name""),
                FOREIGN KEY(""Type"") REFERENCES ""EmergencyType""(""Name""),
	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );
        ";
        //FOREIGN KEY(""AddedBy"") REFERENCES ""Users""(""Id""),
    }
}
