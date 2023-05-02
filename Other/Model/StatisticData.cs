using EmergencySituations.DataBase.Model;

namespace EmergencySituations.Other.Model
{
    public class StatisticData
    {
        public int Date { get; set; }
        public int TotalCount { get; set; }
        public Dictionary<string, int> Level { get; set; }
        public Dictionary<string, int> Type { get; set; }
        public Losses Losses { get; set; }
    }

    public static class Ex
    {

        public static string[,] ToTable(this Dictionary<string, int> d)
        {
            var b = new string[,] { { "", "", "", "" }, { "", "", "", "" } };
            int i = 0;
            d.ToList().ForEach(data =>
             {
                 b[0, i] = data.Key;
                 b[1, i++] = data.Value.ToString();
             });
            return b;
        }
    }

}
