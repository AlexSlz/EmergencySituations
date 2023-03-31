using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Printing;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        [AuthFilter(new string[] {"Користувачі"})]
        [HttpGet("{tableName}")]
        public ActionResult<string> GetTable(string tableName, int id = 0, int limit = 999999)
        {
            var a = new MyDBContext(tableName);
            var find = a.FirstOrDefault(i => i["Код"].ToString() == id.ToString());
            return Ok((find != null) ? find : a.Take(limit));
        }

        [HttpGet("main")]
        public ActionResult<string> GetMainTable(int limit = 0, int page = 1)
        {
            var a = new MyDBContext("Надзвичайні ситуації");
            var type = new MyDBContext("Тип НС");
            var level = new MyDBContext("Рівень НС");
            foreach (var item in a)
            {
                item["Тип"] = type.FindById(item["Тип"])["Назва"];
                item["Рівень"] = level.FindById(item["Рівень"])["Назва"];
            }
            if (limit > 0)
            {
                return Ok(a.Skip((page - 1) * limit)
                      .Take(limit));
            }
            else
            {
                return Ok(a);
            }
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

            return (data.Count <= 0) ? Ok() : Ok(data.Update(tableName));
        }

        [AuthFilter]
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

            return (data.Count <= 0) ? Ok() : Ok(data.Update(tableName));
        }

        [AuthFilter]
        [HttpDelete("{tableName}")]
        public IActionResult Delete(string tableName, object json)
        {
            
            var data = new MyDBContext(json);

            foreach (var item in data)
            {
                if (!MyDataBase.DeleteRowById(tableName, int.Parse(item["Код"].ToString())))
                {
                    return BadRequest("Error Data.");
                }
            }

            return Ok();
        }
    }
}
