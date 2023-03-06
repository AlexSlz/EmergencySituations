using EmergencySituations.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.GetData("SELECT * FROM [Користувачі]");
        }

        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            return this.GetData($"SELECT * FROM [Користувачі] WHERE [Код] = {id}");
        }

        [HttpGet("login/{login}")]
        public IActionResult Get(string login)
        {
            return this.GetData($"SELECT * FROM [Користувачі] WHERE [Логін] = '{login}'");
        }

        [HttpPost]
        public IActionResult Post(object json)
        {
            var result = MyDataBase.AddRow("Користувачі", json);
            return result ? Created("/users", "ok") : Problem("Error");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = MyDataBase.DeleteRow("Користувачі", id);
            return result ? Ok() : Problem("Error");
        }

        [HttpPut("{id}")]
        public IActionResult Put(object json, int id)
        {
            Console.WriteLine(json);
            var result = MyDataBase.UpdateRow("Користувачі", json, id);
            return result ? Ok() : Problem("Error");
        }
    }
}
