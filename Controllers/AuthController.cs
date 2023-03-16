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
            var user = MyDataBase.Users.FirstOrDefault(u => u.Login == req.Login && u.Password == req.Password);
            if (user == null)
                return BadRequest("User Not Found");
            return Token.GenerateToken(user.Login);
        }
    }
}
