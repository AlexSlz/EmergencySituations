using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        [HttpGet("Рік")]
        public ActionResult<string> Year()
        {
            return Ok(GetData("Year"));
        }
        [HttpGet("Місяць")]
        public ActionResult<string> Month()
        {
            return Ok(GetData("Month"));
        }

        private IEnumerable<Dictionary<string, object>> GetData(string type)
        {
            var data = MyDataBase.Select<Emergency>();
            var levels = MyDataBase.Select<EmergencyLevel>().Select(i => i.Name);
            var types = MyDataBase.Select<EmergencyType>().Select(i => i.Name);
            return data.GroupBy(i => i.DateAndTime.GetType().GetProperty(type).GetValue(i.DateAndTime)).Select(i =>
            {
                var date = i.Select(a => a.DateAndTime.GetType().GetProperty(type).GetValue(a.DateAndTime)).First();
                var temp = new Dictionary<string, object> { { "Дата", date }, { "Усього", i.Count() } };

                temp.Add("level", getCount(date, levels, data, type));
                temp.Add("type", getCount(date, types, data, type));

                return temp;
            });

        }

        private Dictionary<string, int> getCount(object date, IEnumerable<string> names, List<Emergency> data, string type)
        {
            var temp = new Dictionary<string, int>();
            foreach (var name in names)
            {
                temp.Add(name, data.Where(i => (i.Level == name || i.Type == name) && int.Parse(i.DateAndTime.GetType().GetProperty(type).GetValue(i.DateAndTime).ToString()) == int.Parse(date.ToString())).Count());
            }
            return temp;
        }
    }
}
