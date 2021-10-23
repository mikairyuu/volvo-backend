using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using volvo_backend.Models;
using volvo_backend.Utils;

namespace volvo_backend.Controllers
{
    [ApiController]
    [Route("route")]
    public class RouteController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<RouteModel> GetRoute([FromQuery(Name = "RouteId")] int routeId)
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand("select * from routetable where route_id=@routeId");
            cmd.Parameters.AddWithValue("@routeId", routeId);
            var reader = dbase.GetReader(cmd);
            var routeModel = new RouteModel
            {
                Id = reader.GetInt32("route_id"),
                Img = reader.GetString("route_img"),
                Distance = reader.GetInt32("route_distance"),
                Title = reader.GetString("route_name"),
                Description = reader.GetString("route_description"),
                UseCount = reader.GetInt32("route_visited"),
                LastUsedAtTS = ((DateTimeOffset) reader.GetMySqlDateTime("route_last_date").Value)
                    .ToUnixTimeSeconds().ToString()
            };
            return routeModel;
        }


        [HttpGet("get/all")]
        public ActionResult<RouteLists> GetAllRoutes()
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand("select * from routetable");
            var reader = dbase.GetReader(cmd);
            var routes = new RouteLists()
            {
                VolvoRoutes = new List<RouteModel>(),
                CustomRoutes = new List<RouteModel>()
            };
            while (reader.Read())
            {
                var routeModel = new RouteModel
                {
                    Id = reader.GetInt32("route_id"),
                    Img = reader.GetString("route_img"),
                    Distance = reader.GetInt32("route_distance"),
                    Title = reader.GetString("route_name"),
                    Description = reader.GetString("route_description"),
                    UseCount = reader.GetInt32("route_visited"),
                    LastUsedAtTS = ((DateTimeOffset) reader.GetMySqlDateTime("route_last_date").Value)
                        .ToUnixTimeSeconds().ToString()
                };

                if (reader.GetBoolean("route_is_volvo"))
                    routes.VolvoRoutes.Add(routeModel);
                else
                    routes.CustomRoutes.Add(routeModel);
            }

            dbase.CloseConnection();
            return routes;
        }

        [HttpGet("get/path")]
        public ActionResult<PathModel> GetPathByRouteId([FromQuery(Name = "RouteId")] int routeId)
        {
            PathModel path;
            try
            {
                var file = System.IO.File.OpenText($"Routes/route{routeId}.json");
                path = new PathModel {Path = file.ReadToEnd()};
            }
            catch (Exception)
            {
                return NotFound(); //TODO: Determine whether it's a good idea
            }

            return path;
        }
    }
}