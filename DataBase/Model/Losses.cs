namespace EmergencySituations.DataBase.Model
{
    public class Losses : IDBTable
    {
        public int Id { get; set; }

        public int EasyAccident { get; set; }
        public int HardAccident { get; set; }
        public int DisabilityPerson { get; set; }
        public int DeathPersonUnderSixty { get; set; }
        public int DeathPersonUndersSixteen { get; set; }
        public int DestroyedBuildings { get; set; }
        public int DamagedPersonalItems { get; set; }
        public int Costs { get; set; }

        public static string Sql => @"
            CREATE TABLE IF NOT EXISTS ""Losses"" (
	            ""Id""	INTEGER NOT NULL,
	            ""EasyAccident""	INTEGER,
	            ""HardAccident""	INTEGER,
	            ""DisabilityPerson""	INTEGER,
	            ""DeathPersonUnderSixty""	INTEGER,
	            ""DeathPersonUndersSixteen""	INTEGER,
	            ""DestroyedBuildings""	INTEGER,
	            ""DamagedPersonalItems""	INTEGER,
	            ""Costs""	INTEGER,

	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );
        ";
    }
}
