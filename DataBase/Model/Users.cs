namespace EmergencySituations.DataBase.Model
{
    public class Users : IDBTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public static string Sql => @"
            CREATE TABLE IF NOT EXISTS ""Users"" (
	            ""Id""	INTEGER NOT NULL,
	            ""Name""	TEXT,
	            ""PhoneNumber""	TEXT,
	            ""Email""	TEXT,
	            ""Login""	TEXT,
	            ""Password""	INTEGER,
	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );
        ";
    }
}
