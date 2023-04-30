using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using EmergencySituations.Other;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(int year = 0)
        {
            var temp = GetData(year);
            if (temp == null)
                return NotFound();
            return Ok(temp);
        }
        

        public static IEnumerable<StatisticData> GetData(int year = 0)
        {
            var data = MyDataBase.Select<Emergency>();
            var levels = MyDataBase.Select<EmergencyLevel>().Select(i => i.Name);
            var types = MyDataBase.Select<EmergencyType>().Select(i => i.Name);
            var yearGroup = data.GroupBy(i => i.DateAndTime.Year);
           
            return yearGroup.Select(i =>
            {
                var date = i.Select(a => a.DateAndTime.Year).First();
                var current = data.Where(i => i.DateAndTime.Year == date);

                if(year > 0)
                {
                    date = i.Select(a => a.DateAndTime.Month).First();
                    current = data.Where(i => i.DateAndTime.Year == year && i.DateAndTime.Month == date);
                    if (current.Count() == 0)
                        return null;
                }

                var result = new StatisticData() { 
                    Date = int.Parse(date.ToString()), 
                    TotalCount = i.Count()  
                };

                result.Level = getCount(current, levels);
                result.Type = getCount(current, types);
                result.Costs = current.Select(i => i.Losses.Costs).Sum();

                
                return result;
            });

        }

        private static Dictionary<string, int> getCount(IEnumerable<Emergency> current, IEnumerable<string> names)
        {
            var temp = new Dictionary<string, int>();
            foreach (var name in names)
            {
                temp.Add(name, current.Where(i => i.Level.Name == name || i.Type.Name == name).Count());
            }
            return temp;
        }
    }
}
