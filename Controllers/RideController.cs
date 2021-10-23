using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using volvo_backend.Models;
using volvo_backend.Utils;

namespace volvo_backend.Controllers
{
    [ApiController]
    [Route("ride")]
    public class RideController : ControllerBase
    {
        [HttpPost("join")]
        public ActionResult<RouteModel> JoinRideById([FromQuery(Name = "RideId")] int routeId, [FromQuery(Name = "UserId")] int userId)
        {
            RouteModel ride = null;
            var dbase = new DBManager();
            var cmd = new MySqlCommand("$SELECT * FROM eventuser where user_id = @id;");
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = dbase.GetReader(cmd);
            if (reader.Read()) return null;
            dbase.CloseReader();
            cmd = new MySqlCommand(
                $"select * from routetable where route_id =  @id;" +
                $"update routetable Set route_visited =route_visited+1 where route_id = @id ;");
            cmd.Parameters.AddWithValue("@id", routeId);
            reader = dbase.GetReader(cmd);
            if (!reader.Read()) return BadRequest();
            ride = new RouteModel()
            {
                Id = reader.GetInt32("route_id"),
                Img = reader.GetString("route_img"),
                Distance = reader.GetInt32("route_distance"),
                Title = reader.GetString("route_name"),
                Description = reader.GetString("route_description"),
                LastUsedAtTS = ((DateTimeOffset)reader.GetMySqlDateTime("route_last_date").Value)
                   .ToUnixTimeSeconds().ToString()
            };
            dbase.CloseReader();
            cmd = new MySqlCommand(
                $"INSERT INTO eventuser(user_id, route_id) VALUES (@user_id,@route_id);");
            cmd.Parameters.AddWithValue("@route_id", routeId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            dbase.InsertCommand(cmd);
            dbase.CloseConnection();
            return ride;

        }

    }
}