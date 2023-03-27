using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        [AuthFilter(new string[] {"Користувачі"})]
        [HttpGet("{tableName}")]
        public ActionResult<string> GetTable(string tableName)
        {
            var a = new MyDBContext(tableName);
            return Ok(a);
        }

        [HttpGet("main")]
        public ActionResult<string> GetMainTable()
        {
            var a = new MyDBContext("Надзвичайні ситуації");
            var type = new MyDBContext("Тип НС");
            var level = new MyDBContext("Рівень НС");
            foreach (var item in a)
            {
                item["Тип"] = type.FindById(item["Тип"])["Назва"];
                item["Рівень"] = level.FindById(item["Рівень"])["Назва"];
            }
            return Ok(a);
        }

        [HttpGet("{tableName}/getLastId")]
        public IActionResult GetLastID(string tableName)
        {
            var data = new MyDBContext(tableName);
            return Ok(data.GetMaxId());
        }

        [HttpGet("{tableName}/getKey")]
        public IActionResult GetKey(string tableName)
        {
            return Ok(MyDataBase.GetKeyTypeTable(tableName));
        }

        [AuthFilter]
        [HttpGet("getTableNameList")]
        public IActionResult Get()
        {
            return Ok(MyDataBase.GetTableNameList());
        }

        [AuthFilter]
        [HttpPost("{tableName}")]
        public IActionResult Post(string tableName, object json)
        {
            var data = new MyDBContext(json);

            foreach (var item in data)
            {
                if (!MyDataBase.AddRow(tableName, item))
                {
                    return BadRequest("Error Data.");
                }
            }

            return Ok(new MyDBContext(tableName).Last());
        }

        [HttpPut("{tableName}")]
        public IActionResult Put(string tableName, object json)
        {
            var data = new MyDBContext(json);

            foreach (var item in data)
            {
                if (!MyDataBase.UpdateRow(tableName, item))
                {
                    return BadRequest("Error Data.");
                }
            }

            return Ok();
        }
    }
}
