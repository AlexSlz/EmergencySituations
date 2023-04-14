namespace EmergencySituations.DataBase.Model
{
    public class Positions : IDBTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public static string Sql => @"
            CREATE TABLE IF NOT EXISTS ""Positions"" (
	            ""Id""	INTEGER NOT NULL,
	            ""Name""	TEXT,
	            ""Location""	TEXT,
	            ""X""	DOUBLE,
	            ""Y""	DOUBLE,
	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );
        ";
    }
}
