using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using EmergencySituations.Other.Model;
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
            var temp = CalculateData(year);
            if (temp == null)
                return NotFound();
            return Ok(temp);
        }

        [HttpGet("year")]
        public ActionResult<string> GetYear(int year = 0)
        {
            if (year != 0)
            {
                var month = MyDataBase.GetMonthList(year);
                month.Sort();
                return Ok(month);
            }
            var years = MyDataBase.GetYearList();
            years.Sort();
            return Ok(years);
        }


        public static IEnumerable<StatisticData> CalculateData(int year = 0)
        {
            var data = MyDataBase.Select<Emergency>();
            var levels = MyDataBase.Select<EmergencyLevel>().Select(i => i.Name);
            var types = MyDataBase.Select<EmergencyType>().Select(i => i.Name);
            var group = data.GroupBy(i => i.DateAndTime.Year);

            if(year > 0)
            {
                group = data.Where(i => i.DateAndTime.Year == year).GroupBy(i => i.DateAndTime.Month);
            }

            var res = group.Select(d =>
            {
                var current = d.ToList();

                var result = new StatisticData() { 
                    Date = d.Key, 
                    TotalCount = current.Count()  
                };

                result.Level = getCount(current, levels);
                result.Type = getCount(current, types);
                var loss = new Losses
                {
                    EasyAccident = current.Select(i => i.Losses.EasyAccident).Sum(),
                    HardAccident = current.Select(i => i.Losses.HardAccident).Sum(),
                    DeathPersonUnderSixty = current.Select(i => i.Losses.DeathPersonUnderSixty).Sum(),
                    DeathPersonUndersSixteen = current.Select(i => i.Losses.DeathPersonUndersSixteen).Sum(),
                    DisabilityPerson = current.Select(i => i.Losses.DisabilityPerson).Sum(),

                    DestroyedBuildings = current.Select(i => i.Losses.DestroyedBuildings).Sum(),
                    DamagedPersonalItems = current.Select(i => i.Losses.DamagedPersonalItems).Sum(),
           
                    Costs = current.Select(i => i.Losses.Costs).Sum()
                };
                result.Losses = loss;


                
                return result;
            });
            return res.ToList().OrderBy(i => i.Date);
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
