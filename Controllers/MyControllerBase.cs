using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    public abstract class MyControllerBase<T> : ControllerBase where T : IDBTable, new()
    {
        [HttpGet]
        public ActionResult<string> GetTableData(int id = 0)
        {
            if (id >= 1)
                return Ok(MyDataBase.Select<T>().Where(x => x.Id == id));
            return Ok(MyDataBase.Select<T>());
        }

        [HttpGet("getKeys")]
        public ActionResult<string> GetKeys()
        {
            return Ok(MyDataBase.GetTableKeys<T>());
        }

        [HttpPost]
        [AuthFilter]
        public virtual ActionResult<string> AddToTable(T data)
        {
            return Ok(MyDataBase.Insert(data).Message);
        }

        [HttpPut]
        [AuthFilter]
        public virtual ActionResult<string> EditTable(T data)
        {
            return Ok(MyDataBase.Update(data).Message);
        }

        [HttpDelete("{id}")]
        [AuthFilter]
        public ActionResult<string> DeleteData(int id)
        {
            return Ok(MyDataBase.Delete<T>(id).Message);
        }

    }
}
