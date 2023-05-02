namespace EmergencySituations.DataBase.Model
{
    public class Losses : IDBTable
    {
        public int Id { get; set; }

        public int AffectedPerson { get; set; }
        public int AffectedAnimals { get; set; }
        public int DestroyedBuildings { get; set; }
        public int DamagedBuildings { get; set; }
        public int DestroyedPersonalItems { get; set; }
        public int DamagedPersonalItems { get; set; }
        public int Costs { get; set; }

        public static string Sql => @"
            CREATE TABLE IF NOT EXISTS ""Losses"" (
	            ""Id""	INTEGER NOT NULL,
	            ""AffectedPerson""	INTEGER,
	            ""AffectedAnimals""	INTEGER,
	            ""DestroyedBuildings""	INTEGER,
	            ""DamagedBuildings""	INTEGER,
	            ""DestroyedPersonalItems""	INTEGER,
	            ""DamagedPersonalItems""	INTEGER,
	            ""Costs""	INTEGER,

	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );
        ";
    }
}
