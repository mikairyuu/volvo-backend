using System;
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
            var result = new List<UserInfo>();
            var dbase = new DBManager();
            var cmd = new MySqlCommand(
                "SELECT * FROM ( SELECT `route_id` FROM `eventuser` WHERE user_id = @id ) " +
                "AS fr JOIN routetable on routetable.route_id = fr.route_id " +
                "JOIN imagetable on imagetable.route_id=fr.route_id");
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = dbase.GetReader(cmd);
            var list = new List<RouteModel>();
            var prevId = -1;
            while (reader.Read())
            {
                if (reader.GetInt32("route_id") != prevId)
                    list.Add(new RouteModel
                    {
                        Id = reader.GetInt32("route_id"),
                        Img = new List<string> { reader.GetString("Img") },
                        Volvos = reader.GetInt32("route_distance"),
                        Title = reader.GetString("route_name"),
                        Description = reader.GetString("route_description"),
                        UseCount = reader.GetInt32("route_visited"),
                        IsParticipant = reader.GetBoolean("route_is_volvo")
                    });
                else
                    list[^1].Img.Add(reader.GetString("Img"));
                prevId = reader.GetInt32("route_id");


            }
            dbase.CloseConnection();

            return list;
        }

        [HttpGet("rating/get")]
        public ActionResult<List<UserInfo>> GetRating()
        {
            var result = new List<UserInfo>();
            var dbase = new DBManager();
            var reader = dbase.GetReader(new MySqlCommand(
                "select usertable.full_name,usertable.Img,usertable.user_id,T.score from usertable" +
                " right join (select * from userstats order by score limit 10) AS T" +
                " on usertable.user_id=T.user_id order by T.score DESC"));
            while (reader.Read())
            {
                result.Add(new UserInfo
                {
                    Id = reader.GetInt32("user_id"),
                    Img = reader.GetString("Img"),
                    Name = reader.GetString("full_name"),
                    Score = reader.GetInt32("score")
                });
            }

            return result;
        }
    }
}