using EmergencySituations.DataBase;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        [HttpGet("getLastId/{name}")]
        public IActionResult GetLastID(string name)
        {
            if (MyDataBase.CountRow(name) == 0)
                return Ok("0");
            else
            {
                return Ok(MyDataBase.ExecuteQueryWithValue($"SELECT MAX(Код) FROM [{name}]"));
            }
        }

        [HttpGet("getKey/{name}")]
        public IActionResult GetKey(string name)
        {
            return Ok(JsonConvert.SerializeObject(MyDataBase.GetKeyTypeTable(name)));
        }

        [HttpGet("getTableNameList")]
        public IActionResult Get()
        {
            return Ok(MyDataBase.GetTableNameList());
        }

        [HttpPost("{name}")]
        public IActionResult Post(string name, object json)
        {
            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());
            MyDataBase.AddRowOld(name, data);
            return Ok();
        }
        [HttpPost("{name}/many")]
        public IActionResult PostMany(string name, object json)
        {
            List<Dictionary<string, object>> data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json.ToString());
            foreach (var item in data)
            {
                MyDataBase.AddRowOld(name, item);
            }
            return Ok();
        }
    }
}
