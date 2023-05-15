using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetTableNameList()
        {
            return Ok(MyDataBase.GetTableNameList());
        }

        [HttpGet("test")]
        public ActionResult<string> Test(int c = 100, int minYear = 2018)
        {
            Random r = new Random();
            for (int i = 0; i < c; i++)
            {
                DateTime date = new DateTime(minYear, 1, 1);
                int range = (DateTime.Today - date).Days;
                date = date.AddDays(r.Next(range));
                var temp = new Emergency()
                {
                    Name = $"Test#{i}",
                    DateAndTime = date,
                    Level = new EmergencyLevel() { Id = r.Next(1, 5) },
                    Type = new EmergencyType() { Id = r.Next(1, 5) },
                    Losses = new Losses() { Costs = r.Next(1000, 50000) },
                };
                MyDataBase.Insert(temp);

                var t = new Positions() { EmergencyId = MyDataBase.GetLastId("Emergency"), X = r.Next(42, 52), Y = r.Next(20, 40) };
                MyDataBase.Insert(t);
            }


            return Ok();
        }

    }
}
