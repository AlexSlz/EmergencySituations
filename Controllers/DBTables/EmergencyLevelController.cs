﻿using EmergencySituations.DataBase.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySituations.Controllers.DBTables
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyLevelController : MyControllerBase<EmergencyLevel>
    {

    }
}
