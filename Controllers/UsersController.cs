using EmergencySituations.DataBase;
using EmergencySituations.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmergencySituations.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MyDataBase.Users);
            //return this.GetData("SELECT * FROM [Користувачі]");
        }

        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            return Ok(MyDataBase.Users.FirstOrDefault(i => i.Код == id));
            //return this.GetData($"SELECT * FROM [Користувачі] WHERE [Код] = {id}");
        }

        [HttpGet("login/{login}")]
        public IActionResult Get(string login)
        {
            return Ok(MyDataBase.Users.FirstOrDefault(i => i.Логін == login));
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            MyDataBase.Users.Add(user);
            MyDataBase.Users.UpdateTable();
            //var result = MyDataBase.AddRow("Користувачі", user);
            return Created($"/users/id/{user.Код}", user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            MyDataBase.Users.Remove(MyDataBase.Users.Find(i => i.Код == id));
            MyDataBase.Users.UpdateTable();
            return Ok();
            //var result = MyDataBase.DeleteRow("Користувачі", id);
            //return result ? Ok() : Problem("Error");
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            var result = MyDataBase.UpdateRow("Користувачі", user);
            return result ? Ok() : Problem("Error");
        }
    }
}
