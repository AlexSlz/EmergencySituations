using DocumentFormat.OpenXml.Math;
using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    public abstract class MyControllerBase<T> : ControllerBase where T : IDBTable, new()
    {
        [HttpGet]
        public ActionResult<string> GetTableData(int id = 0, int limit = 5, int page = 1, string order = "id")
        {
            var maxCount = MyDataBase.Count<T>();
            Response.Headers.Add("totalData", $"{maxCount}");
            decimal d = (decimal)((double)maxCount / (double)limit);
            Response.Headers.Add("maxPage", $"{Math.Ceiling(d)}");
            int offset = (limit * page) - limit;
            string sql = $"SELECT * FROM [{typeof(T).Name}] order by {order} limit {limit} offset {offset}";
            if (id >= 1)
                return Ok(MyDataBase.Select<T>($"SELECT * FROM [{typeof(T).Name}] WHERE id = {id}"));
            return Ok(MyDataBase.Select<T>(sql));
        }

        [HttpPost("search")]
        public ActionResult<string> Search(T data, int limit = 5, int page = 1, string order = "id")
        {
            int offset = (limit * page) - limit;
            var filter = MyDataBase.GetSearchFilter(data);

            string sql = $"SELECT * FROM [{typeof(T).Name}] WHERE {string.Join(" AND ", filter)} order by {order} limit {limit} offset {offset}";
            var result = MyDataBase.Select<T>(sql);
            if (result.Count <= 0)
                return NotFound();

            var totalCount = MyDataBase.Count<T>($"WHERE {string.Join(" AND ", filter)}");
            Response.Headers.Add("totalData", $"{totalCount}");
            decimal d = (decimal)((double)totalCount / (double)limit);
            Response.Headers.Add("maxPage", $"{Math.Ceiling(d)}");
            return Ok(result);
        }

        [HttpGet("getKeys")]
        [AuthFilter]
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
            var res = MyDataBase.Delete<T>(id);
            if(res.isError)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }

    }
}
