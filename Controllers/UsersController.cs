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
            return Ok(MyDataBase.Users.FirstOrDefault(i => i.Id == id));
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
            //var result = MyDataBase.AddRow("Користувачі", user);
            return Created($"/users/id/{user.Id}", user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            MyDataBase.Users.Remove(MyDataBase.Users.Find(i => i.Id == id));
            return Ok();
            //var result = MyDataBase.DeleteRow("Користувачі", id);
            //return result ? Ok() : Problem("Error");
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            MyDataBase.Users[MyDataBase.Users.GetIndex(user)] = user;
            return Ok();
        }
    }
}
