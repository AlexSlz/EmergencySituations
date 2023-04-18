using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers.DBTables
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyController : MyControllerBase<Emergency>
    {
        [HttpPost]
        [AuthFilter]
        public override ActionResult<string> AddToTable(Emergency data)
        {
            List<Positions> positions = data.Positions;
            MyDataBase.Insert(data, "Positions");
            var maxId = MyDataBase.Select<Emergency>().Max(e => e.Id);
            positions.ForEach(position =>
            {
                position.EmergencyId = maxId;
                MyDataBase.Insert(position);
            });

            return "Ok";
        }
    }
}
