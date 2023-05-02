using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using EmergencySituations.Other.Model;
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
            var user = MyDataBase.Select<Users>().FirstOrDefault(u => u.Login == req.Login && u.Password.ToString() == req.Password);

            if (user == null)
                return BadRequest("User Not Found");
            return Ok(Token.GenerateToken(user.Login).Key);
        }
        [HttpGet("{token}")]
        public ActionResult<string> GetTokenOwner(string token)
        {
            var user = MyDataBase.Select<Users>().FirstOrDefault(u => u.Login == Token.GetOwner(token));
            if (user == null)
                return BadRequest("User Not Found");
            user.Password = "";
            return Ok(user);
        }

        [HttpGet("{token}/check")]
        public ActionResult<string> CheckToken(string token)
        {
            return Ok(Token.Validation(token));
        }
    }
}
