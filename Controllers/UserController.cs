using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using volvo_backend.Models;
using volvo_backend.Utils;

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

        [HttpGet("rating/get")]
        public ActionResult<List<UserInfo>> GetRating()
        {
            var result = new List<UserInfo>();
            var dbase = new DBManager();
            var reader = dbase.GetReader(new MySqlCommand(
                "select usertable.full_name,usertable.Img,usertable.user_id from usertable " +
                "right join (select * from userstats order by score limit 10) AS T on usertable.user_id=T.user_id"));
            while (reader.Read())
            {
                result.Add(new UserInfo
                {
                    Id = reader.GetInt32("user_id"),
                    Img = reader.GetString("Img"),
                    Name = reader.GetString("full_name")
                });
            }

            return result;
        }
    }
}