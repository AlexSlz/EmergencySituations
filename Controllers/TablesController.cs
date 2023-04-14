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
            return Ok();
            //return Ok(MyDataBase.GetTableNameList());
        }

        [HttpGet("getKey/{tableName}")]
        public ActionResult<string> GetTableKey(string tableName)
        {
            return Ok();
            //if (!MyDataBase.TablesExists(tableName))
            //    return BadRequest("Table Not Found.");
            //return Ok(MyDataBase.GetTableKey(tableName));
        }
    }
}
