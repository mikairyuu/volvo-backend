using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using volvo_backend.Models;
using volvo_backend.Utils;

namespace volvo_backend.Controllers
{
    [ApiController]
    [Route("ride")]
    public class RideController : ControllerBase
    {
        [HttpPost("join")]
        public ActionResult JoinRideById([FromQuery(Name = "RouteId")] int routeId,
            [FromQuery(Name = "UserId")] int userId)
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand($"SELECT * FROM eventuser where user_id = @id;");
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = dbase.GetReader(cmd);
            if (reader.Read()) { dbase.CloseConnection(); return null; }
            dbase.CloseReader();
            cmd = new MySqlCommand(
                $"update routetable Set route_visited =route_visited+1 where route_id = @id ;");
            cmd.Parameters.AddWithValue("@id", routeId);
            dbase.InsertCommand(cmd);
            cmd = new MySqlCommand(
                $"INSERT INTO eventuser(user_id, route_id) VALUES (@user_id,@route_id);");
            cmd.Parameters.AddWithValue("@route_id", routeId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            dbase.InsertCommand(cmd);
            dbase.CloseConnection();
            return Ok();
        }

        [HttpPost("done")]
        public ActionResult MarkRideDone([FromQuery(Name = "RouteId")] int routeId,
            [FromQuery(Name = "UserId")] int userId)
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand("DELETE FROM eventuser WHERE route_id=@route_id AND user_id=@user_id;" +
                                       "UPDATE userstats SET score=score+(SELECT distance FROM routetable WHERE route_id=@route_id) " +
                                       "WHERE user_id=@user_id");
            cmd.Parameters.AddWithValue("@route_id", routeId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            dbase.InsertCommand(cmd);
            dbase.CloseConnection();
            return Ok();
        }

        [HttpDelete("delete")]
        public ActionResult MarkRideDeleted([FromQuery(Name = "RouteId")] int routeId,
            [FromQuery(Name = "UserId")] int userId)
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand("DELETE FROM eventuser WHERE route_id=@id AND user_id=@user_id");
            cmd.Parameters.AddWithValue("@id", routeId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            dbase.InsertCommand(cmd);
            dbase.CloseConnection();
            return Ok();
        }
    }
}