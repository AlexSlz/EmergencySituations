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
            MyDataBase.Insert(data);
            var maxId = MyDataBase.Select<Emergency>().Max(e => e.Id);
            Position(positions, maxId, MyDataBase.Insert);

            return "Ok";
        }

        [HttpPut]
        [AuthFilter]
        public override ActionResult<string> EditTable(Emergency data)
        {
            if (data.Positions != null)
            {
                var difference = MyDataBase.Select<Emergency>().Find(i => i.Id == data.Id).Positions
                    .FindAll(old =>
                    {
                        return !data.Positions.Any(n => n.Id == old.Id);
                    });
                difference.ForEach(e =>
                {
                    MyDataBase.Delete(e);
                    data.Positions.Remove(e);
                });
            }
            List<Positions> positions = data.Positions;
            MyDataBase.Update(data);
            Position(positions, data.Id, MyDataBase.Update);

            return "Ok";
        }


        private void Position(List<Positions> positions, int id, Func<IDBTable, string> func)
        {
            if (positions != null)
                positions.ForEach(position =>
                {
                    position.EmergencyId = id;
                    func.Invoke(position);
                });
        }
    }
}
