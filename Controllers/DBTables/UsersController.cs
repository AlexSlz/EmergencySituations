using EmergencySituations.Auth;
using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers.DBTables
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthFilter]
    public class UsersController : MyControllerBase<Users>
    {

    }
}
