using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [AuthFilter]
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(MyDataBase.Users);
            //return this.GetData("SELECT * FROM [Користувачі]");
        }

        [HttpGet("id/{id}")]
        public ActionResult<string> Get(int id)
        {
            return Ok(MyDataBase.Users.FirstOrDefault(i => i.Id == id));
            //return this.GetData($"SELECT * FROM [Користувачі] WHERE [Код] = {id}");
        }

        [HttpPost]
        public ActionResult<string> AddUser(User user)
        {
            MyDataBase.Users.Add(user);
            //var result = MyDataBase.AddRow("Користувачі", user);
            return Created($"/users/id/{user.Id}", user);
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            MyDataBase.Users.Remove(MyDataBase.Users.Find(i => i.Id == id));
            return Ok();
            //var result = MyDataBase.DeleteRow("Користувачі", id);
            //return result ? Ok() : Problem("Error");
        }

        [HttpPut]
        public ActionResult<string> Put(User user)
        {
            MyDataBase.Users[MyDataBase.Users.GetIndex(user)] = user;
            return Ok();
        }
    }
}
