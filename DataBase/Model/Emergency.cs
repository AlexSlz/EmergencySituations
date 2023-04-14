namespace EmergencySituations.DataBase.Model
{
    public class Emergency
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAndHour { get; set; }
        public int LevelId { get; set; }
        public int TypeId { get; set; }
        public string Image { get; set; }
        public int Costs { get; set; }
        public List<Positions> Positions { get; set; }
        public int AddedBy { get; set; }
    }
}
