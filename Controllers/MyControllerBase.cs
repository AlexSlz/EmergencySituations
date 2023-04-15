using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    public abstract class MyControllerBase<T> : ControllerBase where T : IDBTable, new()
    {
        [HttpGet]
        public ActionResult<string> GetTableData()
        {
            return Ok(MyDataBase.Select<T>());
        }

        [HttpGet("getKeys")]
        public ActionResult<string> GetKeys()
        {
            return Ok(MyDataBase.GetTableKeys<T>());
        }
        [HttpPost]
        [AuthFilter]
        public ActionResult<string> AddToTable(T data)
        {
            return Ok(MyDataBase.Insert(data));
        }
        [HttpPost("list")]
        public ActionResult<string> AddToTableList(List<T> data)
        {
            return BadRequest("Not Yet.");
        }

        [HttpPut]
        [AuthFilter]
        public ActionResult<string> EditTable(T data)
        {
            return Ok(MyDataBase.Update(data));
        }

        [HttpDelete]
        [AuthFilter]
        public ActionResult<string> DeleteDataFromTable(T data)
        {
            return Ok(MyDataBase.Delete(data));
        }

        [HttpDelete("{id}")]
        [AuthFilter]
        public ActionResult<string> DeleteDataFromTableByKey(int id)
        {
            return Ok(MyDataBase.Delete<Users>(id));
        }

    }
}
