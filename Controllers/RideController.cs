using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using volvo_backend.Models;
using volvo_backend.Services;
using volvo_backend.Utils;

namespace volvo_backend.Controllers
{
    [ApiController]
    [Route("ride")]
    public class RideController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Ride> GetRideById([FromQuery(Name = "RideId")] int rideId)
        {
            var ride = new Ride();
            var dbase = new DBManager();
            var cmd = new MySqlCommand($"select * from eventtable where event_id = @id");
            cmd.Parameters.AddWithValue("@id", rideId);
            var reader = dbase.GetReader(cmd);
            if (!reader.Read())
            {
                return null;
            }
            var routeId = reader.GetInt32("route_id");
            dbase.CloseReader();
            ride.userList = Service.GetUsersByRideId(rideId);
            cmd = new MySqlCommand($"select * from routetable where route_id =  @id");
            cmd.Parameters.AddWithValue("@id", routeId);
            reader = dbase.GetReader(cmd);
            if (reader.Read())
                ride.route = new RouteModel()
                {
                    Id = reader.GetInt32("route_id"),
                    Img = reader.GetString("route_img"),
                    Distance = reader.GetInt32("route_distance"),
                    Title = reader.GetString("route_name"),
                    Description = reader.GetString("route_description"),
                    LastUsedAtTS = ((DateTimeOffset) reader.GetMySqlDateTime("route_last_date").Value)
                        .ToUnixTimeSeconds().ToString()
                };
            dbase.CloseConnection();
            return ride;
        }

        [HttpPost("create")]
        public ActionResult<Ride> GetRideById([Bind("RideId")] RideApplication ride)
        {
            //TODO
            return NotFound();
        }
    }
}