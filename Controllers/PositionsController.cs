using EmergencySituations.DataBase;
using EmergencySituations.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MyDataBase.Positions);
        }

        [HttpPost]
        public IActionResult Post(Position position)
        {
            MyDataBase.Positions.Add(position);
            return Ok(position);
        }

        [HttpPut]
        public IActionResult Put(Position position)
        {
            MyDataBase.Positions[MyDataBase.Positions.GetIndex(position)] = position;
            return Ok();
        }
    }
}
