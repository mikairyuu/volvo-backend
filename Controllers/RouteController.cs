using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
            var cmd = new MySqlCommand("" +
                "SELECT * FROM ( SELECT * FROM routetable WHERE route_id = @routeId  ) AS fr JOIN imagetable on imagetable.route_id = fr.route_id");
            cmd.Parameters.AddWithValue("@routeId", routeId);
            var reader = dbase.GetReader(cmd);
            if (!reader.Read()) return null;
            var routeModel = new RouteModel
            {
                Id = reader.GetInt32("route_id"),
                Img = new List<string> { reader.GetString("Img") },
                Volvos = reader.GetInt32("route_distance"),
                Title = reader.GetString("route_name"),
                Description = reader.GetString("route_description"),
                UseCount = reader.GetInt32("route_visited"),
                IsParticipant = reader.GetBoolean("route_is_volvo")
            };
            while (reader.Read()) routeModel.Img.Add(reader.GetString("Img"));
            return routeModel;
        }


        [HttpGet("get/all")]
        public ActionResult<RouteLists> GetAllRoutes()
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand("SELECT * FROM routetable JOIN imagetable on imagetable.route_id = routetable.route_id order by routetable.route_id");
            var reader = dbase.GetReader(cmd);
            var routes = new RouteLists
            {
                VolvoRoutes = new List<RouteModel>(),
                CustomRoutes = new List<RouteModel>()
            };
            var prevId = -1;
            RouteModel routeModel = null;
            while (reader.Read())
            {
                if (reader.GetInt32("route_id") != prevId)
                {
                    routeModel = new RouteModel
                    {
                        Id = reader.GetInt32("route_id"),
                        Img = new List<string> { reader.GetString("Img") },
                        Volvos = reader.GetInt32("route_distance"),
                        Title = reader.GetString("route_name"),
                        Description = reader.GetString("route_description"),
                        UseCount = reader.GetInt32("route_visited"),
                        IsParticipant = reader.GetBoolean("route_is_volvo")
                    };

                    if (reader.GetBoolean("route_is_volvo"))
                        routes.VolvoRoutes.Add(routeModel);
                    else
                        routes.CustomRoutes.Add(routeModel);
                }
                else
                    routeModel.Img.Add(reader.GetString("Img"));
                prevId = reader.GetInt32("route_id");
            }

            dbase.CloseConnection();
            return routes;
        }

        // [HttpGet("get/path")]
        // public ActionResult<PathModel> GetPathByRouteId([FromQuery(Name = "RouteId")] int routeId)
        // {
        //     PathModel path;
        //     try
        //     {
        //         var file = System.IO.File.OpenText($"Routes/route{routeId}.json");
        //         path = new PathModel {Path = file.ReadToEnd()};
        //     }
        //     catch (Exception)
        //     {
        //         return NotFound(); //TODO: Determine whether it's a good idea
        //     }
        //
        //     return path;
        // }

        [HttpGet("get/detail")]
        public ActionResult<PathModel> GetRouteFeatures([FromQuery(Name = "id")] int id)
        {
            var path = new PathModel();
            try
            {
                var file = System.IO.File.OpenText($"Routes/route{id}.json");
                path.Path = file.ReadToEnd();
            }
            catch (Exception)
            {
                return NotFound(); //TODO: Determine whether it's a good idea
            }

            path.partnerOffers = new List<PartnerOffer>();
            var dbase = new DBManager();
            var cmd = new MySqlCommand("select * from routefeatures where route_id=@id");
            cmd.Parameters.AddWithValue("@id", id);
            var reader = dbase.GetReader(cmd);
            while (reader.Read())
            {
                path.partnerOffers.Add(new PartnerOffer
                {
                    Address = reader.GetString("address"),
                    Discount = reader.GetString("discount"),
                    Title = reader.GetString("title"),
                    Img = reader.GetString("Img")
                });
            }
            
            dbase.CloseReader();
            cmd = new MySqlCommand("" +
                                       "SELECT * FROM ( SELECT * FROM routetable WHERE route_id = @routeId  ) AS fr JOIN imagetable on imagetable.route_id = fr.route_id");
            cmd.Parameters.AddWithValue("@routeId", id);
            reader = dbase.GetReader(cmd);
            reader.Read();
            path.Id = reader.GetInt32("route_id");
            path.Img = new List<string> {reader.GetString("Img")};
            path.Volvos = reader.GetInt32("route_distance");
            path.Title = reader.GetString("route_name");
            path.Description = reader.GetString("route_description");
            path.UseCount = reader.GetInt32("route_visited");
            path.IsParticipant = reader.GetBoolean("route_is_volvo");
            path.Distance = reader.GetInt32("route_distance");
            while (reader.Read()) path.Img.Add(reader.GetString("Img"));
            return path;
        }
    }
}