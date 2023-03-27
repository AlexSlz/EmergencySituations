using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.Model;

using Microsoft.AspNetCore.Mvc;


namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Auth(UserDto req)
        {
            var user = new MyDBContext("Користувачі").FirstOrDefault(u => u["Логін"].ToString() == req.Login && u["Пароль"].ToString() == req.Password);
            if (user == null)
                return BadRequest("User Not Found");
            return Token.GenerateToken(user["Логін"].ToString());
        }
        [HttpGet("{token}")]
        public ActionResult<string> getUserByToken(string token)
        {
            var user = new MyDBContext("Користувачі").FirstOrDefault(u => u["Логін"].ToString() == Token.Decrypt(token));
            if (user == null)
                return BadRequest("User Not Found");
            user.Remove("Пароль");
            return Ok(user);
        }

        [HttpGet("{token}/check")]
        public ActionResult<string> checkToken(string token)
        {
            return Ok(Token.Validation(token));
        }
    }
}
