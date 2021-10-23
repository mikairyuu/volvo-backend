using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using volvo_backend.Models;

namespace volvo_backend.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        [HttpGet("ride/get")]
        public ActionResult<List<RouteModel>> GetRideById([FromQuery(Name = "UserId")] int userId)
        {
            //TODO
            return null;
        }
    }
}