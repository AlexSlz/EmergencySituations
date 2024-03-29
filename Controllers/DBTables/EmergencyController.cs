﻿using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using EmergencySituations.Other.Model;
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
            var a = MyDataBase.Insert(data);
            if (a.isError)
            { 
                return BadRequest(a.Message);
            }

            var maxId = MyDataBase.GetLastId(typeof(Emergency).Name);
            Position(positions, maxId, MyDataBase.Insert);

            return "Ok";
        }

        [HttpPut]
        [AuthFilter]
        public override ActionResult<string> EditTable(Emergency data)
        {
            if (data.Positions != null)
            {
                var find = MyDataBase.Select<Emergency>($"SELECT * FROM Emergency WHERE id = {data.Id}").First();
                if (find != null)
                {
                    var difference = find.Positions
                        .FindAll(old =>
                        {
                            return !data.Positions.Any(n => n.Id == old.Id);
                        });

                    difference.ForEach(e =>
                    {
                        MyDataBase.Delete<Positions>(e.Id);
                        data.Positions.Remove(e);
                    });
                }
            }
            List<Positions> positions = data.Positions;
            var a = MyDataBase.Update(data);
            if (a.isError)
            {
                return BadRequest(a.Message);
            }
            Position(positions, data.Id, MyDataBase.Update);

            return "Ok";
        }


        private void Position(List<Positions> positions, int id, Func<IDBTable, MyRequestResult> func)
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
