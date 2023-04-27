namespace EmergencySituations.DataBase.Model
{
    public interface IDBTable
    {
        public int Id { get; set; }
        public static string Sql
        {
            get;
        }
    }
}
