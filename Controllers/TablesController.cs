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
    }
}
