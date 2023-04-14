﻿namespace EmergencySituations.DataBase.Model
{
    public class EmergencyType : IDBTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static string Sql => @"
            CREATE TABLE ""EmergencyType"" (
	            ""Id""	INTEGER NOT NULL,
	            ""Name""	TEXT NOT NULL UNIQUE,
	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );
        ";
    }
}
